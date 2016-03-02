gem 'albacore', '<1.0.0'

require 'net/http'
require 'fileutils'
require 'albacore'

# Filter out slashes and dots from the branch name
def filter_branch_name(branchName)
  lastIndex = branchName.rindex('/') 
  length = branchName.length
  if (lastIndex == nil)
    return branchName
  end
  lastIndex = lastIndex + 1
  return branchName[lastIndex, length - lastIndex].chomp
end

# Configuration Settings
ilb_version = "2.6.0"
build_number = ENV['BUILD_NUMBER'] || "0"
file_version = "#{ilb_version}.#{build_number}"

sql_packager_scripts_location = 'SqlScripts/SqlPackager'
deployment_location = '\\\\mlctestsrvs2\\Documents\\ReleaseDeployment'
branch_name = filter_branch_name(ENV['branch_name'] || `git rev-parse --abbrev-ref HEAD`)

current_directory = File.dirname(__FILE__)
windows_current_directory = current_directory.gsub("/", "\\")
#update_version_sql = "UPDATE SysParam SET SysParamValue = '" + ilb_version + "' WHERE SysParamId = 47"
release_build = false

visual_studio_version = ENV["VisualStudioVersion"]
if visual_studio_version.nil?
	visual_studio_version = "14.0"
	ENV["VisualStudioVersion"] = visual_studio_version
end
puts "Using VisualStudio Version" + ENV["VisualStudioVersion"]

puts('Running for branch:' + branch_name)
puts('Running for version:' + ilb_version)
puts('Running for build number:' + build_number)

include Comparable

def <=>(other)
  
end

def pipe(*cmd, &block)
  IO.popen(cmd).each_line do |line|
    block.call line unless block.nil?
  end
  err = $?.to_i
  throw "Command failed with status #{err}: #{cmd.join " "}" if err > 0
end

task :ensure_clean_working_directory do
  clean = false
  pipe 'git', 'status' do |line|
    if line == "nothing to commit, working directory clean\n"
      clean = true
    end
  end

  unless clean
    puts "ERROR: Ensure your working directory is clean."
    exit 1
  end
end

task :clean do
  begin
    sh 'git clean -xfd'
  rescue
  end
end

#desc "Restoring nuget packages"
task :restore do 
	puts 'Restoring packages'
  sh '.nuget\NuGet.exe restore .\Backend.sln -source "https://nuget.org/api/v2/"'
	sh '.nuget\NuGet.exe restore .\Services.sln -source "https://nuget.org/api/v2/"'
end

desc "Update nuget packages"
task :update => :restore do ||
#	if (branch_name.start_with?('develop')) 
#		puts 'Updating nuget packages'
#		sh 'Source\.nuget\NuGet.exe update -Safe Source\_BuildSolution.sln -source "http://Solicitors-build:90/nuget/ProGetFeeds"'
#	end
end

task :prerequisites => [:restore, :update] do
  
end

desc "Compile Common"
msbuild :compile_common do | msb |
  msb.properties = { 
  						:configuration => release_build ? :Release : :Debug, 
  						:AllowedReferenceRelatedFileExtensions => release_build ? "none" : ".pdb; .xml",
  						:DebugType=> release_build ? "None" : "pdbonly"
  					}
  msb.targets = [ :Build ]
  msb.command = "C:\\Program Files (x86)\\MSBuild\\" + visual_studio_version + "\\Bin\\msbuild.exe"
  msb.solution = './Common.sln'
  msb.verbosity = :minimal
  msb.max_cpu_count = 4
  msb.other_switches = {:toolsVersion => visual_studio_version}
end


desc "Run assembly file doc production clients"
task :set_documentproduction_clients_version #=> ||
#assemblyinfo :set_documentproduction_clients_version do |asm|
#  asm.version = ENV['BUILD_NUMBER']
#  asm.title = "Solicitors.InteropToOpenXml.Clients"
#  asm.output_file = "Solicitors.InteropToOpenXml/Solicitors.DocumentProduction.Clients/Properties/AssemblyInfo.cs"
#end

desc "Run assembly file doc production service hosts"
task :set_documentproduction_service_hosts_version
#assemblyinfo :set_documentproduction_service_hosts_version do |asm|
#  asm.version = ENV['BUILD_NUMBER']
#  asm.title = "Solicitors.InteropToOpenXml.ServiceHosts"
#  asm.output_file = "Solicitors.InteropToOpenXml/Solicitors.DocumentProduction.ServiceHost/Properties/AssemblyInfo.cs"
#end

desc "Run assembly file doc production"
task :set_documentproduction_version
#assemblyinfo :set_documentproduction_version do |asm|
#  asm.version = ENV['BUILD_NUMBER']
#  asm.title = "Solicitors.InteropToOpenXml.DocProduction"
#  asm.output_file = "Solicitors.InteropToOpenXml/Solicitors.DocumentProduction/Properties/AssemblyInfo.cs"
#end

desc "Compile Services Module"
task :Compile_Services 
msbuild :Compile_Services => [:set_documentproduction_clients_version, :set_documentproduction_service_hosts_version, :set_documentproduction_version,:compile_common ] do | msb |
  msb.properties = { 
            	:configuration => release_build ? :Release : :Debug,
				:AllowedReferenceRelatedFileExtensions => release_build ? "none" : ".pdb; .xml",#
				:DebugType=> release_build ? "None" : "pdbonly"
            }
  msb.targets = [ :Build ]
  msb.command = "C:\\Program Files (x86)\\MSBuild\\" + visual_studio_version + "\\Bin\\msbuild.exe"
  msb.solution = './Services.sln'
  msb.verbosity = :minimal
  msb.max_cpu_count = 4
  msb.other_switches = {:toolsVersion => visual_studio_version}
end

desc "Compile Workflow Module"
msbuild :compile_frontEnd => [ :compile_common ] do | msb |
  msb.properties = { 
             	:configuration => release_build ? :Release : :Debug, 
				:AllowedReferenceRelatedFileExtensions => release_build ? "none" : ".pdb; .xml",
				:DebugType=> release_build ? "None" : "pdbonly"
            }
  msb.targets = [ :Build ]
  msb.command = "C:\\Program Files (x86)\\MSBuild\\" + visual_studio_version + "\\Bin\\msbuild.exe"
  msb.solution = './FrontEnd.sln'
  msb.verbosity = :minimal
  msb.max_cpu_count = 4
end
 
task :restore_diary do 
#  puts 'Restoring NuGet packages for Diary modules'
#  sh 'Solicitors.Diary\.nuget\NuGet.exe restore Solicitors.Diary\Diary.sln -source "https://nuget.org/api/v2/;http://168.62.110.126/api/v2/"'
end

desc "Compile Diary Modules"
msbuild :compile_backend => [ :compile_common, :restore_diary ] do | msb |
  msb.properties = { 
              :configuration => release_build ? :Release : :Debug,         
        :AllowedReferenceRelatedFileExtensions => release_build ? "none" : ".pdb; .xml",
        :DebugType=> release_build ? "None" : "pdbonly"
            }
  msb.targets = [ :Build ]
  msb.command = "C:\\Program Files (x86)\\MSBuild\\" + visual_studio_version + "\\Bin\\msbuild.exe"
  msb.solution = './Backend.sln'
  msb.verbosity = :minimal
  msb.max_cpu_count = 4
  msb.other_switches = {:toolsVersion => visual_studio_version}
end

task :restore_web do 
  puts 'Restoring NuGet packages for Web modules'
##  sh 'Solicitors.Web\.nuget\NuGet.exe restore Solicitors.Web\Web.sln -source "https://nuget.org/api/v2/;http://168.62.110.126/api/v2/"'
end

desc "Compile Web Modules"
task :compile_web
#msbuild :compile_web => [ :compile_common, :compile_backend, :restore_web ] do | msb |
#  msb.properties = { 
#             	:configuration => release_build ? :Release : :Debug, #
##				:platform =>:x86#,#
#				:AllowedReferenceRelatedFileExtensions => release_build ? "none" : ".pdb; .xml",#
#				:DebugType=> release_build ? "None" : "pdbonly"
#           }
#  msb.targets = [ :Build ]
#  msb.command = "C:\\Program Files (x86)\\MSBuild\\" + visual_studio_version + "\\Bin\\msbuild.exe"
#  msb.solution = 'Solicitors.Web/Web.sln'
#  msb.verbosity = :minimal
#  msb.max_cpu_count = 4
#  msb.other_switches = {:toolsVersion => visual_studio_version}
#end

task :compile => [ :compile_common, :compile_backend, :Compile_Services, :compile_frontEnd, :compile_web ]

task :push => [ :ensure_clean_working_directory, :build_and_test ] do
  sh 'git push'
end

task :default => [ :build_and_test ]
task :build_and_test => [ :prerequisites, :set_version_number, :compile, :unit_test ]
task :ci_build => [:release_mode_on, :build_and_test, :binary_layout, :binary_only_release ]
task :release_mode_on do
	release_build = true
end

def check_for_featuretoggle_table_changes(ilb_version)
  puts "Checking SQL Scripts for changes to the feature toggle table..."
  Dir.foreach('SqlScripts/up/ILB' + ilb_version) do |file|
    next if file == '.' or file == '..' or file == ".gitignore" or File.directory? file
    if File.read('SqlScripts/up/ILB' + ilb_version + '/' + file).downcase.include?("featuretoggle")
        puts "ERROR " + file + " changes the feature toggle table!"
        exit 1
    end
  end
end

require 'rexml/streamlistener'
class AssemblyInfoLinkFinder
  include REXML::StreamListener

  def initialize(csproj, search)
    @csproj = csproj
    @expected = File.expand_path search
    @actual = nil
    @found = false
    @block = nil
  end

  attr_reader :found

  def tag_start(name, attrs)
    return if @found

    if name == 'Compile'
      @actual = nil
      return if attrs.assoc('Include').nil?
      @actual = File.expand_path("#{File.dirname @csproj}/#{attrs.assoc('Include')[1]}")
    end

    if name == 'Link'
      exec_on_text do |text|
        @found = (text == "AssemblyInfoProduct.cs" and
              @actual == @expected)
      end
    end
  end

  def text(text_content)
    return if @block.nil?
    @block.call text_content
    @block = nil
  end

  def exec_on_text(&block)
    @block = block
  end
end

desc 'Check all projects have the correct version number'
task :check_for_versioning do
  search = "Source/IRIS.Law.PmsCommonData/AssemblyInfoProduct.cs"
  count = 0
  FileList['Source/**/*.csproj'].each do |csproj|
    assembly_info_link_finder = AssemblyInfoLinkFinder.new csproj, search
    REXML::Document.parse_stream(File.new(csproj, 'r'), assembly_info_link_finder)
    unless assembly_info_link_finder.found
      count += 1
      puts "ERROR: #{csproj} does not reference #{search}"
    end
  end
  if count > 0
    raise "Refusing to continue until projects are fixed."
  end
end

task :write_assembly_info_product do
#  target = 'Source/IRIS.Law.PmsCommonData/AssemblyInfoProduct.cs'
#  input = File.read target
#  assembly_version_matcher = /^\[assembly: AssemblyVersion\(/
#  assembly_file_version_matcher = /^\[assembly: AssemblyFileVersion\(/
#  assembly_informational_version_matcher = /^\[assembly: AssemblyInformationalVersion\(/
#  File.open target, 'w' do |fp|
#    input.each_line do |line|
#      if assembly_version_matcher.match(line)
#        fp.write "[assembly: AssemblyVersion(\"#{ilb_version}\")] // Line written by Rakefile\n"
#      elsif assembly_file_version_matcher.match(line)
#        fp.write "[assembly: AssemblyFileVersion(\"#{ilb_version}.#{build_number}\")] // Line written by Rakefile\n"
#      elsif assembly_informational_version_matcher.match(line)
#        fp.write "[assembly: AssemblyInformationalVersion(\"#{branch_name.chomp}\")] // Line written by Rakefile\n"
#      else
#        fp.write line
#      end
#    end
#  end
end

task :write_wf_web_config do
#  target = 'Source/Solicitors.ServiceHost/App.config'
#  input = File.read target
#  File.open target, 'w' do |fp|
#    input.each_line do |line|
#      if line.include? 'Line written by Rakefile'
#        fp.write "        <bindingRedirect oldVersion=\"0.0.0.0-#{ilb_version}.0\" newVersion=\"#{ilb_version}.0\" /><!-- Line written by Rakefile -->\n"
#      else
#        fp.write line
#      end
#    end
#  end
end

desc 'Set version number for project'
task :set_version_number => [ :check_for_versioning, :write_assembly_info_product, :write_wf_web_config ]


task :display_results_location do 
  puts "##teamcity[importData type='mstest' path='Binaries/Tests/results.trx']"
end

task :unit_test => [ :display_results_location, :mstest, :xunit ] do
  #puts "##teamcity[importData type='mstest' path='Binaries/Tests/results.trx']"
end

desc "MSTest Test Runner Example"
task :mstest 
#mstest :mstest do |mstest|
#    rm_f "Binaries/Tests/results.trx"
#    mstest.command = "C:/Program Files (x86)/Microsoft Visual Studio " + visual_studio_version + "/Common7/IDE/mstest.exe"
#    mstest.assemblies FileList["Binaries/Tests/*.UnitTest.dll", "Binaries/**/IRIS.Law.DocumentProduction.Test.dll", "Binaries/Tests/*Tests.dll"]
#    mstest.parameters "/resultsfile:Binaries/Tests/results.trx"
#end

desc "Run xunit tests"
xunit :xunit => [:xunit_runner] do | xunit |
  xunit.command = "Source/packages/xunit.runner.console.2.0.0/tools/xunit.console.x86.exe"
  xunit.assemblies = FileList["Binaries/Tests/*Tests.dll", "Binaries/Tests/*Test.dll", "Binaries/Tests/*.Tests.dll"]
end

desc "Install xunit runner"
task :xunit_runner do 
  sh 'Source\.nuget\NuGet.exe install  -SolutionDirectory Source -OutputDirectory Source\packages xunit.runner.console -Version 2.0.0'
end

desc 'Build Sql Packages'
task :build_sql_package do
  #copy("#{current_directory}\\IRIS.Law.SchemaUpdates.dll", "#{windows_current_directory}\\Binaries\\SqlPackager\\IRIS.Law.SchemaUpdates.dll")

  #mkdir_p "Binaries/SqlPackager/Updates"
  #copy "Libraries/ILBSetup/FillerInterface.dll", "Binaries/SqlPackager/Updates/"
  #copy "Libraries/ILBSetup/SDLT4.dll", "Binaries/SqlPackager/Updates/"
  #copy "Libraries/ILBSetup/WordViewer_Full.ocx", "Binaries/SqlPackager/Updates/"
  #copy "Libraries/PIA/Microsoft.Office.Interop.Excel.dll", "Binaries/SqlPackager/Updates/"
  #copy "Libraries/PIA/Microsoft.Office.Interop.Outlook.dll", "Binaries/SqlPackager/Updates/"
  #copy "Libraries/PIA/Microsoft.Office.Interop.Word.dll", "Binaries/SqlPackager/Updates/"
  #copy "Libraries/PIA/Microsoft.Vbe.Interop.dll", "Binaries/SqlPackager/Updates/"
  #copy "Libraries/PIA/Office.dll", "Binaries/SqlPackager/Updates/"
  #copy "Libraries/System.Management.Automation.dll", "Binaries/SqlPackager/Updates/"

  #sh "xcopy \"#{windows_current_directory}\\Binaries\\OutlookAddin\" \"#{windows_current_directory}\\Binaries\\SqlPackager\\Updates\\\" /Y /H /R /EXCLUDE:PackagerExcludes.txt"
  #sh "xcopy \"#{windows_current_directory}\\Binaries\\ILBClient\" \"#{windows_current_directory}\\Binaries\\SqlPackager\\Updates\\\" /Y /H /R /EXCLUDE:PackagerExcludes.txt"
  #copy "#{windows_current_directory}\\Binaries\\ILBClient\\IRIS.Law.PmsUpd.exe", "#{windows_current_directory}\\Binaries\\SqlPackager\\Updates\\IRIS.Law.PmsUpd_New.exe"
end

desc "Copy ILB Web Service"
task :copy_ilb_web_service do
  #xcopy_with_config("#{current_directory}\\Solicitors.Web\\IRIS.Law.WebServices", "#{windows_current_directory}\\binaries\\ILBWebService\\")
end

desc 'Perform a release of the software'
task :release, :instance, :dontcheckforfeaturetogglescripts  do |t, args|
  instance = args[:instance]
  featuretogglecheck = args[:dontcheckforfeaturetogglescripts]
  if instance.nil?
    puts "ERROR: provide an instance, i.e. rake #{t.name}[.]"
    puts "       or rake #{t.name}[localhost\\sql2005]"
    exit 1
  end

  override = nil
  if featuretogglecheck.nil?
    override = false
  elsif featuretogglecheck == "true"
    override = true
  end

  if !override
    check_for_featuretoggle_table_changes(ilb_version)
  end

  unless `osql -E -S #{instance} -Q "SELECT @@VERSION"`.include? "2005"
    puts "ERROR: SQL Server Instance must be 2005."
    exit 1
  end

  # Get latest scripts number from SQL Packager
  remote_sql_dir = "#{sql_packager_scripts_location}/ILB#{ilb_version}"
  mkdir_p remote_sql_dir
  version_number = get_sql_latest_version("#{remote_sql_dir}/*.sql") + 1

  # Find all scripts that have been ran.
  clean_directory('SqlScripts/out')
  Dir.rmdir('SqlScripts/out')
  nuget_install("roundhouse")
  sh "Source/packages/roundhouse.0.8.6/bin/rh.exe -db IrisLawBusiness_Deploy -f SqlScripts -o SqlScripts/out -s #{instance} --silent --SearchAllSubdirectoriesInsteadOfTraverse --restore --rfp SqlScripts/2005_MsPms.bak -w true"
  sh 'sqlcmd -E -S ' + instance + ' -d IrisLawBusiness_Deploy -Q "' + update_version_sql + '"' 

  # Copy the scripts that where ran in order to Packager Folder
  version_number = copy_sql_scripts(version_number, remote_sql_dir, "SqlScripts/out/**/up/ILB**/*.sql")
  version_number = copy_sql_scripts(version_number, remote_sql_dir, "SqlScripts/out/**/Functions/*.sql")
  version_number = copy_sql_scripts(version_number, remote_sql_dir, "SqlScripts/out/**/Views/*.sql")
  version_number = copy_sql_scripts(version_number, remote_sql_dir, "SqlScripts/out/**/Sprocs/*.sql")
  version_number = copy_sql_scripts(version_number, remote_sql_dir, "SqlScripts/out/**/Indexes/*.sql")
  version_number = copy_sql_scripts(version_number, remote_sql_dir, "SqlScripts/out/**/Permissions/*.sql")

  # Move all the up files into the Archive folder
  archive_dir = "SqlScripts/up/Archive/ILB#{ilb_version}"
  mkdir_p archive_dir
  (Dir.glob('SqlScripts/out/**/*.sql').select { |path| path.include? '/itemsRan/up/' }).each do |sql_script|
    file_info = File.split(sql_script)
    mv "#{sql_script}", "#{archive_dir}/#{file_info[1]}"
  end

  # Run Script Collector re baseline the db and drop it from the local machine
  sh "Libraries\\ScriptCollector.exe #{ilb_version} IRIS.Law.SchemaUpdates.dll #{sql_packager_scripts_location}/"
  sh "osql -E -S #{instance} -Q \"BACKUP DATABASE [IrisLawBusiness_Deploy] TO  DISK = N'#{current_directory}\\SqlScripts\\2005_MsPms.bak' WITH NOFORMAT, INIT,  NAME = N'IrisLawBusiness-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10\""
  sh "Source/packages/roundhouse.0.8.6/bin/rh.exe -db IrisLawBusiness_Deploy -s #{instance} --drop  --silent"

  # Git Commit the changes
  sh 'git add IRIS.Law.SchemaUpdates.dll'
  FileList["SqlScripts/up/ILB" + ilb_version + "/*.sql"].each { |script| rm script }
  sh 'git add -A SqlScripts'
  sh 'git commit -m "Archive Roundhouse scripts and create new SqlPackager dll."'
end

desc 'Copy Sql Script to Packager'
task :copy_sql_script_to_packager do

  check_for_featuretoggle_table_changes

  # Get latest scripts number from SQL Packager
  remote_sql_dir = "#{sql_packager_scripts_location}/ILB#{ilb_version}"
  mkdir_p remote_sql_dir
  version_number = get_sql_latest_version("#{remote_sql_dir}/*.sql") + 1

  # Copy the scripts that where ran in order to Packager Folder
  version_number = copy_sql_scripts(version_number, remote_sql_dir, "SqlScripts/up/ILB**/*.sql")
  version_number = copy_sql_scripts(version_number, remote_sql_dir, "SqlScripts/Functions/*.sql")
  version_number = copy_sql_scripts(version_number, remote_sql_dir, "SqlScripts/Views/*.sql")
  version_number = copy_sql_scripts(version_number, remote_sql_dir, "SqlScripts/Sprocs/*.sql")
  version_number = copy_sql_scripts(version_number, remote_sql_dir, "SqlScripts/Indexes/*.sql")
  version_number = copy_sql_scripts(version_number, remote_sql_dir, "SqlScripts/Permissions/*.sql")
end

task :generate_db_dll => :copy_sql_script_to_packager do 
  sh "Libraries\\ScriptCollector.exe #{ilb_version} IRIS.Law.SchemaUpdates.dll #{sql_packager_scripts_location}/"
  
  # Copy SchemaUpdates.dll to binaries location
  copy("#{current_directory}\\IRIS.Law.SchemaUpdates.dll", "#{windows_current_directory}\\Binaries\\SqlPackager\\IRIS.Law.SchemaUpdates.dll")
end

task :copy_appsylist_to_client do
  copy("#{windows_current_directory}\\Libraries\\Infragistics\\AppStylist\\AdvancedStyle.isl", "#{windows_current_directory}\\Binaries\\ILBClient\\AdvancedStyle.isl")
end

task :copy_laserform_ocx do
	copy("Libraries\\ILBSetup\\Viewform2.ocx", "Binaries\\ILBClient")
end


task :binary_layout => [ :copy_laserform_ocx, :build_sql_package, :copy_ilb_web_service, :deploy_savedocument_wordaddin, :copy_appsylist_to_client ]
task :installshield => [ :installshield_server_msicab, :installshield_user_msicab ]

directory 'Binaries/Installers'

def installshield_build_copy(configuration, release, ism, *args)
  sh 'c:/Program Files (x86)/InstallShield/2013 SP1 SAB/System/ISCmdBld.exe', '-p', ism, '-a', configuration, '-r', release, '-n', '-b', 'c:\\Install', *args
  puts "INSTALLER FILE COPY"
  puts "c:/Install/#{configuration}/#{release}/DiskImages/DISK1"
  puts "Binaries/Installers/#{configuration} #{release}"  
  cp_r "c:/Install/#{configuration}/#{release}/DiskImages/DISK1", "Binaries/Installers/#{configuration} #{release}"
  puts "INSTALLER FILE COPY COMPLETED"
end

desc 'InstallShield: Server Side MSI & Cab'
task :installshield_server_msicab => 'Binaries/Installers' do
  installshield_build_copy "Server Side 32", "MSI & Cab", "Installers\\Server.ism"\
end

desc 'InstallShield: User Side MSI & Cab'
task :installshield_user_msicab => 'Binaries/Installers' do
  installshield_build_copy "User Side 32", "MSI & Cab", "Installers\\Client.ism", '-z', 'ALB_IS_CLIENT_ONLY=1', '-z', 'SQL_USERNAME=IRISLaw', '-z', 'SQL_PASSWORD=1r15l4w09!'
end

desc 'Copy Binaries'
task :binary_only_release  do
  copy_server_binaries(windows_current_directory, "Binaries\\test_deployment")
  rm_rf "Binaries/test_deployment/server/binaries/SqlScripts/SqlPackager"
end

def copy_sql_scripts(version_number, destination, path) 
  Dir.glob(path).sort().each do |sql_script|
    script_name = File.split(sql_script)[1]
    old_script_name = script_name
    script_name = script_name.split('_')[1] if /([0-9][0-9][0-9][0-9]_[a-zA-Z]*).sql/ =~ script_name
    new_script = "#{version_number}_#{script_name}"
    puts old_script_name + " -> " + new_script

    copy(sql_script, "#{destination}/#{new_script}")
    version_number = version_number + 1
  end 
  return version_number
end


def get_sql_latest_version(version_folder)
  latest_version_number = 0
  Dir.glob(version_folder).each do |sql_script|
    script_name = File.split(sql_script)[1]
    version_number = script_name.split('_')[0].to_i
    latest_version_number = version_number if (version_number > latest_version_number) 
  end
  return latest_version_number.to_i
end

desc 'Deploy Save Document Word Addin'
task :deploy_savedocument_wordaddin do
  tmp_dir = 'Binaries\\tmp'
  solicitors_wordaddin_ver = nil

  matcher = /Solicitors\.WordAddin ([0-9\.]+)/									   
  sh 'Source/.nuget/NuGet.exe install Solicitors.WordAddin -ExcludeVersion -source "https://nuget.org/api/v2/" -source "http://Solicitors-build:90/nuget/ProGetFeeds" -o Binaries\\tmp'
  
  [
    'Microsoft.Owin\lib\net40\*.*',
    'Microsoft.Owin.Diagnostics\lib\net40\*.*',
    'Microsoft.Owin.FileSystems\lib\net40\*.*',
    'Microsoft.Owin.Host.HttpListener\lib\net40\*.*',
    'Microsoft.Owin.Hosting\lib\net40\*.*',
    'Microsoft.Owin.StaticFiles\lib\net40\*.*',
    'Newtonsoft.Json\lib\net45\*.*',
    'Owin\lib\net40\*.*',
    'RestSharp\lib\net4\*.*',
    "Solicitors.WordAddin\\lib\\net40\\*.*",
	"Solicitors.WordAddin\\lib\\net45\\*.*"
  ].each do |glob|
    xcopy("#{tmp_dir}\\#{glob}", 'Binaries\\ILBClient\\')
  end
  
  rm_rf tmp_dir
end

def copy_server_binaries(current_directory, deployment_directory)
  make_directory("#{deployment_directory}\\server\\")
  clean_directory("#{deployment_directory}\\server\\binaries\\")
  make_directory("#{deployment_directory}\\server\\binaries\\")

  nuget_install("roundhouse")
  xcopy("#{current_directory}\\SqlScripts", "#{deployment_directory}\\server\\binaries\\SqlScripts\\")
  copy("#{current_directory}\\Source\\packages\\roundhouse.0.8.6\\bin\\rh.exe","#{deployment_directory}\\server\\binaries\\SqlScripts\\")

  # Copy ILB Web Service
  xcopy("#{current_directory}\\Solicitors.Web\\IRIS.Law.WebServices", "#{deployment_directory}\\server\\binaries\\ILBWebService\\")

  # Copy ILB Client
  xcopy("#{current_directory}\\Binaries\\ILBClient", "#{deployment_directory}\\server\\binaries\\ILBClient\\")  
  xcopy("#{current_directory}\\Binaries\\TaskScheduler", "#{deployment_directory}\\server\\binaries\\TaskScheduler\\")

  # Copy Self Host Services
  xcopy("#{current_directory}\\Binaries\\SelfHostServices", "#{deployment_directory}\\server\\binaries\\SelfHostServices\\") 
end

def nuget_install(package_name) 
  sh "Source\\.nuget\\NuGet.exe install  -SolutionDirectory Source -OutputDirectory Source\\packages #{package_name}"
end

def xcopy(source_directory, target_directory)
  sh "xcopy \"#{source_directory}\" \"#{target_directory}\" /Y /H /R /Z /E /EXCLUDE:excludes.txt"
end

def xcopy_with_config(source_directory, target_directory)
  sh "xcopy \"#{source_directory}\" \"#{target_directory}\" /Y /H /R /Z /E /EXCLUDE:PackagerExcludes.txt"
end

def make_directory(directory_name)
  if (Dir.exist?(directory_name) == false)
    sh "mkdir \"#{directory_name}\""
  end
end

def 
  clean_directory(directory_name)
  if (Dir.exist?(directory_name) == false)
    sh "mkdir \"#{directory_name}\""
  end
  Dir.foreach(directory_name) {|f| fn = File.join(directory_name, f); FileUtils.rm_r(fn) if f != '.' && f != '..' }
end

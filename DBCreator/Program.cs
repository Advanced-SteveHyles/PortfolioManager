using System;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading;
using Portfolio.BackEnd.Repository;

namespace DBCreator
{
    class Program
    {                
        static void Main(string[] args)
        {
            var databasePath = Environment.CurrentDirectory;
            string RawConnection =
                    $"Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename={databasePath}\\PortfolioManagerDummy.mdf;Initial Catalog=TestConnection;Integrated Security=True";
            

            using (var ctx = new PortfolioManagerContext(RawConnection, true))
            {                
                var x = from x1 in ctx.PortfolioValuations select x1;
                ctx.SaveChanges();                                
            }
            
        }
    }
}

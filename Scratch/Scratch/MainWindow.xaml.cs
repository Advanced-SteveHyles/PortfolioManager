using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scratch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static public ObservableCollection<Topic> Topics = new ObservableCollection<Topic>();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new TextDataContext();

                
            Topic Level0A = new Topic("S", 4);
            Level0A.ChildTopics.Add(new AccountTopic("Summary", -1));
            Level0A.ChildTopics.Add(new AccountTopic("Level 0:2", -1));
            var item = new AccountTopic("Account 1", -1);
            Level0A.ChildTopics.Add(item);
            item.GrandChildTopics.Add(new Topic("Level 0:3:1GC", -1));

            var item4 = new AccountTopic("Account 2", -1);
            Level0A.ChildTopics.Add(item4);
            item4.GrandChildTopics.Add(new InvestmentTopic("Transactions", -1));
            item4.GrandChildTopics.Add(new FundTopic("Funds", -1));

            Topics.Add(Level0A);

            Topic Level0B = new Topic("M", 4);
            Topics.Add(Level0B);


            myTreeView.DataContext = Topics;
        }

        private void MyTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int i = 1;
        }
    }

    public class FundTopic : ITopic
    {
        
        public FundTopic(string funds, int i)
        {
            Title = funds;
            Rating = i;
        }

        public string Title { get; set; }
        public int Rating { get; set; }
    }
}


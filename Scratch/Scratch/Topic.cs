using System.Collections.ObjectModel;

namespace Scratch
{
    public class Topic: ITopic
    {
        public string Title { get; set; }
        public int Rating { get; set; }
        private ObservableCollection<AccountTopic> childTopicsValue = new ObservableCollection<AccountTopic>();
                
        public ObservableCollection<AccountTopic> ChildTopics
        {
            get
            {
                return childTopicsValue;
            }
            set
            {
                childTopicsValue = value;
            }
        }
        public Topic() { }
        public Topic(string title, int rating)
        {
            Title = title;
            Rating = rating;
        }
    }


    public class AccountTopic
    {
        public string Title { get; set; }
        public int Rating { get; set; }
        private ObservableCollection<ITopic> grandchildTopicsValue = new ObservableCollection<ITopic>();


        public ObservableCollection<ITopic> GrandChildTopics
        {
            get
            {
                return grandchildTopicsValue;
            }
            set
            {
                grandchildTopicsValue = value;
            }
        }
       
        public AccountTopic() { }
        public AccountTopic(string title, int rating)
        {
            Title = title;
            Rating = rating;
        }
    }

    public interface ITopic
    {
        string Title { get; set; }
        int Rating { get; set; }
    }

    public class InvestmentTopic: ITopic
    {
        public string Title { get; set; }
        public int Rating { get; set; }
        private ObservableCollection<Topic> grandchildTopicsValue = new ObservableCollection<Topic>();


        public ObservableCollection<Topic> GrandChildTopics
        {
            get
            {
                return grandchildTopicsValue;
            }
            set
            {
                grandchildTopicsValue = value;
            }
        }

        public InvestmentTopic() { }
        public InvestmentTopic(string title, int rating)
        {
            Title = title;
            Rating = rating;
        }
    }
}
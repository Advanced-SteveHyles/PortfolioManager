using System.Collections.ObjectModel;

namespace Scratch
{
    public class Topic
    {
        public string Title { get; set; }
        public int Rating { get; set; }
        private ObservableCollection<ChildTopic> childTopicsValue = new ObservableCollection<ChildTopic>();
                
        public ObservableCollection<ChildTopic> ChildTopics
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


    public class ChildTopic
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
       
        public ChildTopic() { }
        public ChildTopic(string title, int rating)
        {
            Title = title;
            Rating = rating;
        }
    }
}
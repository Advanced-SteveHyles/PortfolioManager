using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Scratch
{
    public class TextDataContext
    {
        public ObservableCollection<ITreeViewThings> TreeViewItems {
            get
            {
                return new ObservableCollection<ITreeViewThings>(new List<ITreeViewThings>()
                {
                    new TopLevelTreeViewThings(),
                    new SubLevelTreeViewThings(),
                    new TopLevelTreeViewThings(),
                    new SubLevelTreeViewThings(),
                    new TopLevelTreeViewThings(),
                    new TopLevelTreeViewThings(),

                }

                    );
            }
    }

    }

    public class TopLevelTreeViewThings : ITreeViewThings
    {
    }

        public class SubLevelTreeViewThings : ITreeViewThings
        {
        }

        
        public interface ITreeViewThings
    {
    }



    public class Topic
    {
        public string Title { get; set; }
        public int Rating { get; set; }
        private ObservableCollection<Topic> childTopicsValue = new ObservableCollection<Topic>();
        private ObservableCollection<Topic> GrandchildTopicsValue = new ObservableCollection<Topic>();


        public ObservableCollection<Topic> GrandChildTopics
        {
            get
            {
                return GrandchildTopicsValue;
            }
            set
            {
                GrandchildTopicsValue = value;
            }
        }

        public ObservableCollection<Topic> ChildTopics
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

}
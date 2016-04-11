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
}
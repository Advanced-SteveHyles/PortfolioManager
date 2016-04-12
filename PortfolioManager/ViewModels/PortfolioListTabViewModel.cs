using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using PortfolioManager.Model;
using PortfolioManager.UIBuilders;

namespace PortfolioManager.ViewModels
{
    public class PortfolioListTabViewModel
    {                
       
        public PortfolioDriver PortfolioTreeDataContext
        {
            get { return new PortfolioDriver(); }
        }
    }
}
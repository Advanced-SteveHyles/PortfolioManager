using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms.VisualStyles;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Views.DataEntry;
using PortfolioManager.Views.TabPanels;

namespace PortfolioManager
{
    public class InvestmentsTabsViewModel
    {
        public object InvestmentList
        {
            get { return new InvestmentTabPanel() { DataContext = new InvestmentTabPanelViewModel() }; }
        }


    }

    public class InvestmentTabPanelViewModel
    {
        public ObservableCollection<InvestmentDto> Investments
        {
            get
            {
                var investmentDtos = new List<InvestmentDto>()
                {
                    
                    new InvestmentDto()
                    {
                        Class   ="Class"
                    }                    
                };
                return new ObservableCollection<InvestmentDto>(investmentDtos) ;
            }
            set
            {
            }
        }

    }
}
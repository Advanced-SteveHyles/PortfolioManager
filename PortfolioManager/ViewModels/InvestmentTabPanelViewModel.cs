using System.Collections.ObjectModel;
using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
{
    public class InvestmentTabPanelViewModel
    {
        public ObservableCollection<InvestmentDto> Investments
        {
            get
            {
                return new ObservableCollection<InvestmentDto>(InvestmentModel.GetInvestments());
            }
        }

    }
}
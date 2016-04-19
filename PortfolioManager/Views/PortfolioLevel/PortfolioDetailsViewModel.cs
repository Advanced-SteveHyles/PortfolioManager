using Portfolio.Common.DTO.DTOs;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
{
    public class PortfolioDetailsViewModel
    {
        private readonly PortfolioValuationDto _portfolioValuation;
        public string PropertyTarget { get; set; } = "100";

        public string BondsTarget { get; set; } = "100";
        public string EquityTarget { get; set; } = "100";
        public string CashTarget { get; set; } = "100";
        
        public decimal PropertyActual => _portfolioValuation.PropertyValue;

        public decimal BondsActual => _portfolioValuation.BondValue;
        public decimal EquityActual => _portfolioValuation.EquityValue;
        public decimal CashActual => _portfolioValuation.CashValue;


        public decimal PropertyRatio => _portfolioValuation.PropertyRatio;
        public decimal BondsRatio => _portfolioValuation.BondRatio;
        public decimal EquityRatio => _portfolioValuation.EquityRatio;
        public decimal CashRatio => _portfolioValuation.CashRatio;

        public PortfolioDetailsViewModel(int portfolioId)
        {
            _portfolioValuation = PortfolioModel.GetPortfolioValuation(portfolioId);
        }
    }
}
using Portfolio.API.Virtual;
using Portfolio.API.Virtual.VirtualControllers;

namespace PortfolioManager.Model
{
    public static class PriceHistoryModel
    {
        public static void UpdatePriceHistories()
        {            
            var service = new VirtualPriceHistoryController();
            service.UpdateAllPrices();
        }
    }
}
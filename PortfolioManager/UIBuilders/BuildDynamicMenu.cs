using System;
using PortfolioManager.ViewModels;
using PortfolioManager.Views.DataEntry;

namespace PortfolioManager.UIBuilders
{
    public class BuildDynamicMenu
    {
        public static object PortFolioDataEntryView(Action DialogClose)
        {
            return  new PortfolioDataEntry() { DataContext = new PortfolioDataEntryViewModel(DialogClose) };
        }

        public static object InvestmentDataEntryView(Action DialogClose)
        {
            return new InvestmentDataEntry() { DataContext = new InvestmentDataEntryViewModel(DialogClose) };
        }

        public static object PriceHistoryDataEntryView(Action DialogClose)
        {
            return new MassPriceUpdateDataEntry() { DataContext = new MassPriceUpdateDataEntryViewModel(DialogClose) };
        }
    }
}
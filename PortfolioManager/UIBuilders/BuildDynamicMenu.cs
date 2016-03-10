using System;
using PortfolioManager.Views.DataEntry;

namespace PortfolioManager.ViewModels.Menus
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
    }
}
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
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;
using PortfolioManager.Model.Decorators;

namespace PortfolioManager.ViewModels
{
    public class MassPriceUpdateDataEntryViewModel : AbstractSaveCancelCommands
    {
        public ObservableCollection<PriceHistoryDecorator> AllInvestments
        {
            get
            {
                var investments = InvestmentModel.GetInvestmentsForPriceUpdate();
                return new ObservableCollection<PriceHistoryDecorator>(investments);
            }
        }

        private readonly Action _dialogClose;

        public MassPriceUpdateDataEntryViewModel(Action dialogClose)
        {
            this._dialogClose = dialogClose;
        }

        private void Cancel()
        {
            this._dialogClose.Invoke();
        }
    }
}
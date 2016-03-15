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

        private readonly Action _dialogClose;
        private List<PriceHistoryDecorator> _investments;

        public ObservableCollection<PriceHistoryDecorator> AllInvestments
        {
            get
            {
                _investments = InvestmentModel.GetInvestmentsForPriceUpdate();
                return new ObservableCollection<PriceHistoryDecorator>(_investments);
            }
        }

        public MassPriceUpdateDataEntryViewModel(Action dialogClose)
        {
            this._dialogClose = dialogClose;
            SetCommands(Save, Cancel);
        }

        private void Save()
        {

            PriceHistoryModel.MassSavePriceHistories(_investments);

            this._dialogClose.Invoke();
        }

        private void Cancel()
        {
            this._dialogClose.Invoke();
        }
    }
}
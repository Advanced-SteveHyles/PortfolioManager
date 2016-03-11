using System;
using System.Collections.ObjectModel;
using Portfolio.Common.Constants.Funds;
using Portfolio.Common.DTO.Requests;
using PortfolioManager.Interfaces;
using PortfolioManager.Model;

namespace PortfolioManager.ViewModels
{
    public class InvestmentDataEntryViewModel : AbstractSaveCancelCommands
    {
        public ObservableCollection<string> InvestmentTypes
            => new ObservableCollection<string>(FundInvestmentTypes.InvestmentTypeList);

        public ObservableCollection<string> IncomeTypes
            => new ObservableCollection<string>(FundIncomeTypes.IncomeTypeList);

        public ObservableCollection<string> ClassTypes 
            => new ObservableCollection<string>(FundClasses.FundClassList);

        private readonly Action _dialogClose;

        public int InvestmentId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public string Class { get; set; }
        public string IncomeType { get; set; }
        public string MarketIndex { get; set; }


        public InvestmentDataEntryViewModel(Action dialogClose) : base()
        {
            this._dialogClose = dialogClose;
            SetCommands(Save, Cancel);
        }

        
        private void Save()
        {
            var investmentRequest = new InvestmentRequest()
            {
                Name = this.Name,                
                Symbol = this.Symbol,                
                Type= this.Type,
                Class = this.Class,
                IncomeType = this.IncomeType,
                MarketIndex = this.MarketIndex
            };;

            InvestmentModel.InsertInvestment (investmentRequest);

            this._dialogClose.Invoke();
        }

        private void Cancel()
        {
            this._dialogClose.Invoke();
        }
    }
}
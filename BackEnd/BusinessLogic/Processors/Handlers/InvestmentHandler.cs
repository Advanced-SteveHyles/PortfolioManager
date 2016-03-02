using System.Collections.Generic;
using System.Linq;
using Interfaces;
using PortfolioManager.DTO;
using PortfolioManager.Repository;
using PortfolioManager.Repository.Entities;
using PortfolioManager.Repository.Interfaces;

namespace BusinessLogic.Processors.Handlers
{
    public class InvestmentHandler : IInvestmentHandler
    {
        private readonly IInvestmentRepository _repository;

        public InvestmentHandler(IInvestmentRepository investmentRepository)
        {
            this._repository = investmentRepository;
        }

        public InvestmentDto GetInvestment(int investmentId)
        {
            return _repository.GetInvestment(investmentId).MapToDto();
        }

        public IEnumerable<InvestmentDto> GetInvestments()
        {
            return _repository.GetInvestments().Select(inv=>EntityToDtoMap.MapToDto((Investment) inv));
        }
    }
}
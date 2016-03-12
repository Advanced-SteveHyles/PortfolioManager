using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.DTO.DTOs;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Handlers
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
            return _repository.GetInvestments()
                .ToList()
                .Select(inv=>EntityToDtoMap.MapToDto((Investment) inv));
        }
    }
}
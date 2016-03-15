using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Factories;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualInvestmentsController
    {
        private readonly IInvestmentRepository _repository;

        public VirtualInvestmentsController()
        {
            var connection = ApiConstants.VirtualApiPortfoliomanagercontext;
               _repository = new InvestmentRepository(connection);
        }

        public IEnumerable<InvestmentDto> GetAllInvestments()
        {            
                var results = _repository.GetInvestments()
                    .ToList()
                    .Select(p => p.MapToDto());
                return results;
            
        }

        public InvestmentDto InsertInvestment(InvestmentRequest investmentRequest)
        {
                var entityInvestment = new InvestmentFactory().CreateInvestment(investmentRequest);
                
                var result = _repository.InsertInvestment(entityInvestment);
                if (result.Status == RepositoryActionStatus.Created)
                {
                    var dtoInvestment = result.Entity.MapToDto();
                    return dtoInvestment;
                }

            throw new NotSupportedException("Resolve this");
        }
    }
}
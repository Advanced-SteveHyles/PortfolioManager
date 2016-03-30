using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Factories;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualPortfoliosController
    {
        readonly IPortfolioRepository _repository;

        public VirtualPortfoliosController()
        {
            var connection = ApiConstants.VirtualApiPortfoliomanagercontext;
            _repository = new PortfolioRepository(connection);
            Tracer.Trace(this.ToString());
        }

        public IEnumerable<PortfolioDto> GetPortfolios()
        {            
                var portfolios = _repository.GetPortfolios();
                var portfolioDtos = portfolios.ToList()
                    .Select(p => p.MapToDto());
                return portfolioDtos;         
        }

        public PortfolioDto InsertPortfolio(PortfolioRequest portfolio)
        {

                var entityPortfolio = new PortfolioFactory().CreatePortfolio(portfolio);                
                var result = _repository.InsertPortfolio(entityPortfolio);
                if (result.Status == RepositoryActionStatus.Created)
                {
                    var dtoPortfolio = result.Entity.MapToDto();
                    return dtoPortfolio;
                }

            throw new NotSupportedException();
        }

    }

}

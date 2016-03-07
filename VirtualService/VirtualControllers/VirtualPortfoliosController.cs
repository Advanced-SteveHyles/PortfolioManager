using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.API.Virtual.VirtualActionResults;
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

        public VirtualPortfoliosController(string connection)
        {
            _repository = new PortfolioRepository(connection);
            Tracer.Trace(this.ToString());
        }

        public IVirtualActionResult Get()
        {
            try
            {
                var portfolios = _repository.GetPortfolios();
                var portfolioDtos = portfolios.ToList()
                    .Select(p => p.MapToDto());
                return new OkMultipleActionResult<PortfolioDto>(portfolioDtos);
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return new InternalServerErrorActionResult();
            }
        }

        public IVirtualActionResult Post(PortfolioRequest portfolio)
        {
            try
            {
                if (portfolio == null)
                {
                    return new BadRequestActionResult();
                }

                var entityPortfolio = new PortfolioFactory().CreatePortfolio(portfolio);
                if (entityPortfolio == null)
                {
                    return new BadRequestActionResult();
                }

                /*
                {
                    "userId": "https://expensetrackeridsrv3/embedded_1",
                    "title": "STV",
                    "description": "STV",
                    "expenseGroupStatusId": 1,
                }
                */

                var result = _repository.InsertPortfolio(entityPortfolio);
                if (result.Status == RepositoryActionStatus.Created)
                {
                    var dtoPortfolio = result.Entity.MapToDto();
                    return new CreatedActionResult<PortfolioDto>(dtoPortfolio);
                }
                else
                {
                    return new BadRequestActionResult();
                }

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return new InternalServerErrorActionResult();
            }
        }

    }

}

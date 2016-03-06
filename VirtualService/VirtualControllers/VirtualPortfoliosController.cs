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
        
        public IVirtualActionResult Get(int page = 1, int pageSize = ApiConstants.MaxPageSize)
        {
            try
            {
                // ensure the page size isn't larger than the maximum.
                if (pageSize > ApiConstants.MaxPageSize)
                {
                    pageSize = ApiConstants.MaxPageSize;
                }

                IQueryable<BackEnd.Repository.Entities.Portfolio> portfolios = _repository.GetPortfolios();

                // calculate data for metadata
                var totalCount = portfolios.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var portfolioDtos = portfolios.ToList()
                    .Select(p => p.MapToDto());
                return new  OkMultipleActionResult<PortfolioDto>(portfolioDtos);

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return new InternalServerErrorActionResult();
            }
        }

        public IVirtualActionResult Get(int id, string fields = null)
        {
            try
            {
                bool includeAccounts = false;
                List<string> lstOfFields = new List<string>();

                if (fields != null)
                {
                    lstOfFields = fields.ToLower().Split(',').ToList();
                    includeAccounts = lstOfFields.Any(f => f.Contains("accounts"));
                }


                BackEnd.Repository.Entities.Portfolio portfolio = null;

                if (includeAccounts)
                {
                    portfolio = _repository.GetPortfolioWithAccounts(id);
                }
                else
                {
                    portfolio = _repository.GetPortfolio(id);
                }

                var result = _repository.GetPortfolios().SingleOrDefault(r => r.PortfolioId == id);

                if (result != null)
                {
                    return new OkSingleActionResult<Object>(ShapedData.CreateDataShapedObject(portfolio, lstOfFields));
                }
                else
                {
                    return new NotFoundActionResult();
                }
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
                    return new  BadRequestActionResult();
                }

                var entityPortfolio = new PortfolioFactory().CreatePortfolio(portfolio);
                if (entityPortfolio == null)
                {
                    return new  BadRequestActionResult();
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
                    return new CreatedActionResult(dtoPortfolio);
                }
                else
                {
                    return new  BadRequestActionResult();
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
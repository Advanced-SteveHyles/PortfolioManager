using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using Interfaces;
using Newtonsoft.Json;
using PortfolioManager.DTO;
using PortfolioManager.Repository;
using PortfolioManager.Repository.Entities;
using PortfolioManager.Repository.Factories;
using PortfolioManager.Repository.Interfaces;
using Portfolio_API.Controllers.Transactions;

namespace Portfolio_API.Controllers
{
    public class InvestmentsController : ApiController
    {
        private readonly IInvestmentRepository _repository;

        public InvestmentsController()
        {
            _repository = new InvestmentRepository(new PortfolioManagerContext());
        }
        

        [Route(ApiPaths.Investments, Name = "InvestmentsList")]
        public IHttpActionResult Get(int page = 1, int pageSize = ApiConstants.MaxPageSize)
        {
            try
            {
                // ensure the page size isn't larger than the maximum.
                if (pageSize > ApiConstants.MaxPageSize)
                {
                    pageSize = ApiConstants.MaxPageSize;
                }

                IQueryable<Investment> results = _repository.GetInvestments();
                
                // calculate data for metadata
                var totalCount = results.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var urlHelper = new UrlHelper(Request);
                var prevLink = page > 1
                    ? urlHelper.Link("InvestmentsList",
                        new
                        {
                            page = page - 1,
                            pageSize = pageSize,
                        })
                    : "";
                var nextLink = page < totalPages
                    ? urlHelper.Link("InvestmentsList",
                        new
                        {
                            page = page + 1,
                            pageSize = pageSize,
                        })
                    : "";

                var paginationHeader = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPages = totalPages,
                    previousPageLink = prevLink,
                    nextPageLink = nextLink
                };


                HttpContext.Current.Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));


                return Ok(
                        results
                        //.Skip(pageSize * (page - 1))
                        //.Take(pageSize)
                        );

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return InternalServerError();
            }
        }

        [System.Web.Http.HttpPost]
        [Route(ApiPaths.Investments)]
     
        public IHttpActionResult Post([FromBody] InvestmentRequest investmentRequest)
        {
            try
            {
                if (investmentRequest == null)
                {
                    return BadRequest();
                }

                var entityInvestment = new InvestmentFactory().CreateInvestment(investmentRequest);
                if (entityInvestment == null)
                {
                    return BadRequest();
                }

                /*
                {
                    "userId": "https://expensetrackeridsrv3/embedded_1",
                    "title": "STV",
                    "description": "STV",
                    "expenseGroupStatusId": 1,
                }
                */

                var result = _repository.InsertInvestment(entityInvestment);
                if (result.Status == RepositoryActionStatus.Created)
                {
                    var dtoInvestment = result.Entity.MapToDto();
                    return Created(Request.RequestUri + "/" + dtoInvestment.InvestmentId, dtoInvestment);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return InternalServerError();
            }
        } 
    }
}
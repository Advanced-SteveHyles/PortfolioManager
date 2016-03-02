using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Interfaces;
using PortfolioManager.DTO;
using PortfolioManager.Repository;
using PortfolioManager.Repository.Entities;
using PortfolioManager.Repository.Factories;
using PortfolioManager.Repository.Interfaces;
using PortfolioManager.Repository.Repositories;

namespace Portfolio_API.Controllers
{
    public class AccountsController : ApiController
    {
        IAccountRepository _repository;

        public AccountsController()
        {
            _repository = new AccountRepository(new PortfolioManagerContext());
        }

        public IHttpActionResult Get(int id, string fields = null)
        {
            try
            {
                bool includeInvestments = false;
                List<string> lstOfFields = new List<string>();

                // we should include expenses when the fields-string contains "expenses"
                if (fields != null)
                {
                    lstOfFields = fields.ToLower().Split(',').ToList();
                    includeInvestments = lstOfFields.Any(f => f.Contains("investments"));
                }

                Account accountEnt;
                if (includeInvestments)
                {
                    accountEnt = _repository.GetAccountWithInvestments(id);
                }
                else
                {
                    accountEnt = _repository.GetAccount(id);
                }

                var result = _repository.GetAccount(id);

                if (result != null)
                {

                    return Ok(ShapedData.CreateDataShapedObject(accountEnt, lstOfFields));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return InternalServerError();
            }
        }

        [System.Web.Http.HttpPost]
        [Route(ApiPaths.Accounts)]
        public IHttpActionResult Post([FromBody] AccountRequest account)
        {
            try
            {
                if (account == null)
                {
                    return BadRequest();
                }

                var entityAccount   = AccountFactory.CreateAccount(account);
                if (entityAccount == null)
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

                var result = _repository.InsertAccount(entityAccount);
                if (result.Status == RepositoryActionStatus.Created)
                {
                    var dtoAccount = result.Entity.MapToDto();
                    return Created(Request.RequestUri + "/" + dtoAccount.AccountId, dtoAccount);
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

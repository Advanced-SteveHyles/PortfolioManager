﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Interfaces;
using Portfolio.API.WebApi.Temporary;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Factories;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.API.WebApi.Controllers
{
    public class AccountsController : ApiController
    {
        readonly IAccountRepository _repository;

        public AccountsController()
        {
            _repository = new AccountRepository(ApiConstants.Portfoliomanagercontext);
        }

        public IHttpActionResult Get(int id, string fields = null)
        {
            try
            {
                var includeInvestments = false;
                var lstOfFields = new List<string>();

                // we should include expenses when the fields-string contains "expenses"
                if (fields != null)
                {
                    lstOfFields = fields.ToLower().Split(',').ToList();
                    includeInvestments = lstOfFields.Any(f => f.Contains("investments"));
                }

                var accountEnt = includeInvestments 
                    ? _repository.GetAccountWithInvestments(id) 
                    : _repository.GetAccountByAccountId(id);

                var result = _repository.GetAccountByAccountId(id);

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

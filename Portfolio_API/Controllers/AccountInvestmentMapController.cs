﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Interfaces;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Factories;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.API.WebApi.Controllers
{
    public class AccountInvestmentMapController : ApiController
    {
        private readonly InvestmentRepository _investmentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountInvestmentMapRepository _accountInvestmentRepository;

        public AccountInvestmentMapController()
        {            
            _investmentRepository = new InvestmentRepository(ApiConstants.Portfoliomanagercontext);
            _accountRepository = new AccountRepository(ApiConstants.Portfoliomanagercontext);
            _accountInvestmentRepository = new AccountInvestmentMapRepository(ApiConstants.Portfoliomanagercontext);
        }

        public IHttpActionResult Get(int id)
        {
            var map = new AccountInfoWithAllInvestmentDto();

            var accountEnt = _accountRepository.GetAccountByAccountId(id);
            map.AccountInfo = accountEnt.MapToDto();

            var investmentEntities = _investmentRepository.GetInvestments();
            map.Investments = new List<InvestmentDto>();
            foreach (var investment in investmentEntities.ToList())
            {
                map.Investments.Add(investment.MapToDto());
            }

            try
            {
                //return Ok(ShapedData.CreateDataShapedObject(accountEnt, lstOfFields));
                return Ok(map);
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return BadRequest();
            }
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route(ApiPaths.InvestmentMap)]
        public IHttpActionResult Post([FromBody] AccountInvestmentMapRequest accountInvestmentMapRequest)
        {
            try
            {
                if (accountInvestmentMapRequest == null)
                {
                    return BadRequest();
                }

                var accountInvestmentMap = InvestmentMapFactory.CreateAccountInvestmenMap(accountInvestmentMapRequest);
                if (accountInvestmentMap == null)
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

                var result = _accountInvestmentRepository.InsertAccountInvestmentMap(accountInvestmentMap);
                if (result.Status == RepositoryActionStatus.Created)
                {
                    var dtoAccountInvestmentMap = result.Entity.MapToDto(string.Empty);
                    return Created(Request.RequestUri + "/" + dtoAccountInvestmentMap.AccountInvestmentMapId, dtoAccountInvestmentMap);
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

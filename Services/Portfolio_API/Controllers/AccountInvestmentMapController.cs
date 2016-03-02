using System;
using System.Collections.Generic;
using AccountInvestmentMapDto = PortfolioManager.DTO.DTOs.AccountInvestmentMapDto;

namespace Portfolio_API.Controllers
{
    public class AccountInvestmentMapController : ApiController
    {
        private readonly InvestmentRepository _investmentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountInvestmentMapRepository _accountInvestmentRepository;

        public AccountInvestmentMapController()
        {
            _investmentRepository = new InvestmentRepository(new PortfolioManagerContext());
            _accountRepository = new AccountRepository(new PortfolioManagerContext());
            _accountInvestmentRepository = new AccountInvestmentMapRepository(new PortfolioManagerContext());
        }

        public IHttpActionResult Get(int id)
        {
            var map = new AccountInfoWithAllInvestmentDto();

            var accountEnt = _accountRepository.GetAccount(id);
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
                    var dtoAccountInvestmentMap = result.Entity.MapToDto();
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

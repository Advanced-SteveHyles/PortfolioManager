using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.API.Virtual.VirtualActionResults;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualAccountInvestmentMapController
    {
        private readonly InvestmentRepository _investmentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountInvestmentMapRepository _accountInvestmentRepository;

        private readonly IAccountInvestmentMapRepository _repository;

        public VirtualAccountInvestmentMapController(string connection)
        {
            _accountInvestmentRepository = new AccountInvestmentMapRepository(connection);
        }

        public IVirtualActionResult GetInvestmentsForAccount(int accountId)
        {
            try
            {   
                var investmentEntities = _accountInvestmentRepository.GetAccountInvestmentMapsByInvestmentId(accountId);
                var investments = investmentEntities
                    .ToList()
                    .Select(investment => investment.MapToDto())
                    .ToList();

                try
                {
                    //return Ok(ShapedData.CreateDataShapedObject(accountEnt, lstOfFields));
                    return new OkMultipleActionResult<AccountInvestmentMapDto>(investments);
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError(ex);
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

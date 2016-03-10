using System;
using System.Linq;
using Portfolio.API.Virtual.VirtualActionResults;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualAccountInvestmentMapController
    {
        private readonly IAccountInvestmentMapRepository _accountInvestmentRepository;

        public VirtualAccountInvestmentMapController(string connection)
        {
            _accountInvestmentRepository = new AccountInvestmentMapRepository(connection);
        }

        public IVirtualActionResult GetInvestmentsForAccount(int accountId)
        {
            try
            {
                var accountInvestmentMaps = _accountInvestmentRepository.GetAccountInvestmentMapsByAccountId(accountId);
                var accountInvestmentMap = accountInvestmentMaps
                    .ToList()
                    .Select(investment => investment.MapToDto())
                    .ToList();

                try
                {
                    return new OkMultipleActionResult<AccountInvestmentMapDto>(accountInvestmentMap);
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

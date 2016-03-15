using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualAccountInvestmentMapController
    {
        private readonly IAccountInvestmentMapRepository _accountInvestmentRepository;

        public VirtualAccountInvestmentMapController()
        {
            var connection = ApiConstants.VirtualApiPortfoliomanagercontext;
            _accountInvestmentRepository = new AccountInvestmentMapRepository(connection);
        }

        public List<AccountInvestmentMapDto> GetInvestmentsForAccount(int accountId)
        {

            var accountInvestmentMaps = _accountInvestmentRepository.GetAccountInvestmentMapsByAccountId(accountId);
            var accountInvestmentMap = accountInvestmentMaps
                .ToList()
                .Select(investment => investment.MapToDto(Cache.GetInvestmentName(investment.InvestmentId)))
                .ToList();

            return accountInvestmentMap;

        }
    }
}

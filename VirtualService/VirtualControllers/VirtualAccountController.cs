using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.DTOs.Transactions;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualAccountController
    {
        public readonly IAccountRepository _repository;

        public VirtualAccountController()
        {
            var connection = ApiConstants.VirtualApiPortfoliomanagercontext;
            _repository = new AccountRepository(connection);
        }
        
        public AccountDto GetAccount(int accountId)
        {
                var account = _repository.GetAccountByAccountId(accountId);                    
                return account.MapToDto();            
        }

        public IEnumerable<AccountDto>  GetAccountsForPortfolio(int portfolioId)
        {
            var accounts = _repository.GetAccounts();
            var accountDtos = accounts
                .Where(acc=>acc.PortfolioId == portfolioId)
                .ToList()
                .Select(p => p.MapToDto());
            return accountDtos;
        }
    }
}
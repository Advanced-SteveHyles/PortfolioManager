using System;
using System.Linq;
using Portfolio.API.Virtual.VirtualActionResults;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs;
using Portfolio.Common.DTO.DTOs.Transactions;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class VirtualAccountController
    {
        readonly IAccountRepository _repository;

        public VirtualAccountController()
        {
            var connection = ApiConstants.VirtualApiPortfoliomanagercontext;
            _repository = new AccountRepository(connection);
        }
        
        public IVirtualActionResult Get(int id, string fields = null)
        {
            try
            {
                var accounts = _repository.GetAccounts();                
                var accountDtos = accounts.ToList()
                    .Select(p => p.MapToDto());
                return new OkMultipleActionResult<AccountDto>(accountDtos);
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return new InternalServerErrorActionResult();
            }
        }

        public IVirtualActionResult GetAccount(int accountId)
        {
            try
            {
                var account = _repository.GetAccount(accountId);                    
                return new OkSingleActionResult<AccountDto>(account.MapToDto());
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return new InternalServerErrorActionResult();
            }
        }
    }
}
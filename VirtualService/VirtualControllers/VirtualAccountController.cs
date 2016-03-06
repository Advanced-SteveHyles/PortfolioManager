using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.API.Virtual.VirtualActionResults;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.BackEnd.Repository.Repositories;
using Portfolio.Common.DTO.DTOs;

namespace Portfolio.API.Virtual.VirtualControllers
{
    
    public class VirtualAccountController
    {
        IAccountRepository _repository;

        public VirtualAccountController(string connection)
        {
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

    }
}
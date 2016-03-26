using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfolio.Common.DTO.DTOs;

namespace PortfolioManager.Model.Decorators
{
    public class AccountDtoDecorator
    {
        private readonly AccountDto _account;
        public int AccountId => _account.AccountId;

        public override string ToString()
        {
            return _account.Name;
        }

        public AccountDtoDecorator(AccountDto account)
        {
            _account = account;
        }
    }
}

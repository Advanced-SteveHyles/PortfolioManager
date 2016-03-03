using System;
using System.Collections.Generic;

namespace PortfolioManager.DTO.DTOs.Transactions
{
    public class AccountTransactionSummaryDto
    {
        public int AccountId;

        public ICollection<TransactionDto> Transactions;

    }
}
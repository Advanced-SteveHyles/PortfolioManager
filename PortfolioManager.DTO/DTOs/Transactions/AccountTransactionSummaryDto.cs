using System.Collections.Generic;

namespace Portfolio.Common.DTO.DTOs.Transactions
{
    public class AccountTransactionSummaryDto
    {
        public int AccountId;

        public ICollection<CashTransactionDto> Transactions;

    }
}
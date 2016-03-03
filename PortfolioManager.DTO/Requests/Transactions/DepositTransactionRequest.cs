using System;

namespace Portfolio.Common.DTO.Requests.Transactions
{
    public class DepositTransactionRequest
    {
       public int AccountId { get; set; }

       public DateTime TransactionDate { get; set; }
        
       public decimal Value { get; set; }

       public string Source { get; set; }        

       public bool IsTaxRefund { get; set; }
    }
}

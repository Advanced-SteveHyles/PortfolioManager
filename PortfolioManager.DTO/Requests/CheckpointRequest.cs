using System;
using System.Collections.Generic;
using Portfolio.Common.DTO.DTOs.Transactions;

namespace Portfolio.Common.DTO.Requests
{
    public class CheckpointRequest : ITransactionRequest
    {
        public int AccountId { get; }
        public DateTime FromDate { get; }
        public DateTime ToDate { get; }
        public List<CashTransactionDto> ItemsToCheckpoint { get; }
        public string Reference { get; set; }

        public CheckpointRequest(int accountId, DateTime fromDate, DateTime toDate, List<CashTransactionDto> itemsToCheckpoint, string checkpointReference)
        {
            AccountId = accountId;
            FromDate = fromDate;
            ToDate = toDate;
            ItemsToCheckpoint = itemsToCheckpoint;
            Reference = checkpointReference;
        }
    }
}
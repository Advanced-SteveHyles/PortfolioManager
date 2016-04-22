using System;
using System.Collections.Generic;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.BusinessLogic.Processors.Handlers
{
    public class CheckpointRequest: ITransactionRequest
    {
        public int AccountId { get; }
        public DateTime FromDate { get; }
        public DateTime ToDate { get; }
        public List<int> ItemsToCheckpoint { get; }

        public CheckpointRequest(int accountId ,DateTime fromDate,DateTime toDate,List<int> itemsToCheckpoint)
        {
            AccountId = accountId;
            FromDate = fromDate;
            ToDate = toDate;
            ItemsToCheckpoint = itemsToCheckpoint;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;
using Portfolio.Common.DTO.Requests;

namespace BusinessLogicTests.Fakes
{
    public class FakeCheckpointRepository : ICheckpointRepository
    {
        private int _nextCashCheckpointId;
        private readonly List<CashCheckpoint> _dummyCashCheckpoints = new List<CashCheckpoint>();

        public CashCheckpoint GetCheckpointByCheckpointId(int checkpointId)
        {
            return _dummyCashCheckpoints.FirstOrDefault(cp => cp.CashCheckpointId == checkpointId);
        }

        public RepositoryActionResult<CashCheckpoint> InsertCheckpoint(CheckpointRequest request, decimal openingValue, decimal closingTotal)
        {
            _nextCashCheckpointId++;
            var dummyCheckpoint = new CashCheckpoint()
            {
                AccountId = request.AccountId,
                CashCheckpointId = _nextCashCheckpointId,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                OpeningValue = openingValue,
                ClosingValue = closingTotal,
                Reference = request.Reference
            };

            _dummyCashCheckpoints.Add(dummyCheckpoint);

            return new RepositoryActionResult<CashCheckpoint>(dummyCheckpoint, RepositoryActionStatus.Ok);
        }

        public CashCheckpoint GetLastestCheckpointForAccount(int accountId)
        {
            return _dummyCashCheckpoints
                        .Where(cp => cp.AccountId == accountId)
                        .OrderByDescending(cp=>cp.CashCheckpointId)
                        .FirstOrDefault();
        }
    }
}
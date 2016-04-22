using System;
using System.Collections.Generic;
using System.Linq;
using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.Repository;
using Portfolio.BackEnd.Repository.Entities;
using Portfolio.BackEnd.Repository.Interfaces;

namespace BusinessLogicTests.Fakes
{
    public class FakeCheckpointRepository : ICheckpointRepository
    {
        private int _nextCashCheckpointId;
        private List<CashCheckpoint> _dummyCashCheckpoints = new List<CashCheckpoint>();

        public CashCheckpoint GetCheckpointByCheckpointId(int checkpointId)
        {
            return _dummyCashCheckpoints.Where(cp => cp.CashCheckpointId == checkpointId).FirstOrDefault();
        }

        public RepositoryActionResult<CashCheckpoint> InsertCheckpoint(CheckpointRequest request)
        {
            _nextCashCheckpointId++;
            var dummyCheckpoint = new CashCheckpoint()
            {
                CashCheckpointId = _nextCashCheckpointId,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };

            _dummyCashCheckpoints.Add(dummyCheckpoint);

            return new RepositoryActionResult<CashCheckpoint>(dummyCheckpoint, RepositoryActionStatus.Ok);
        }
    }
}
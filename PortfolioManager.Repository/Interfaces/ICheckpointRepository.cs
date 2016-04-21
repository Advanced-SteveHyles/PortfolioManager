using Portfolio.BackEnd.BusinessLogic.Processors.Handlers;
using Portfolio.BackEnd.Repository.Entities;

namespace Portfolio.BackEnd.Repository.Interfaces
{
    public interface ICheckpointRepository
    {
        CashCheckpoint GetCheckpointByCheckpointId(int checkpointId);
        RepositoryActionResult<CashCheckpoint> InsertCheckpoint(CheckpointRequest request);
    }
}
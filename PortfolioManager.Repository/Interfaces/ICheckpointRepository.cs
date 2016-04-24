using Portfolio.BackEnd.Repository.Entities;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.Repository.Interfaces
{
    public interface ICheckpointRepository
    {
        CashCheckpoint GetCheckpointByCheckpointId(int checkpointId);
        RepositoryActionResult<CashCheckpoint> InsertCheckpoint(CheckpointRequest request, decimal openingValue, decimal closingTotal);
        CashCheckpoint GetLastestCheckpointForAccount(int accountId);
    }
}
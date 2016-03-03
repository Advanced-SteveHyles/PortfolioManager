using System;

namespace Portfolio.BackEnd.Repository
{
    public class RepositoryActionResult<T> : Entities.Portfolio
    {
        public T Entity { get; private set; }
        public RepositoryActionStatus Status { get; private set; }

        public Exception Exception { get; private set; }
        
        public RepositoryActionResult(T entity, RepositoryActionStatus status)
        {
            Entity = entity;
            Status = status;
        }

        public RepositoryActionResult(T entity, RepositoryActionStatus status, Exception exception) : this(entity, status)
        {
            Exception = exception;
        }
    }
}
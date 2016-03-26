using System;
using Portfolio.Common.DTO.Requests;

namespace Portfolio.BackEnd.BusinessLogic.Interfaces
{
    public abstract class BaseProcess<T> where T : ITransactionRequest
    {
        private readonly T _request;

        protected BaseProcess(T request)
        {
            _request = request;
        }

        public void Execute()
        {
            if (!Validate(_request))
            {
                throw new InvalidOperationException("Request Is Not Valid");
            }

            ProcessToRun();
            ExecuteResult = true;
        }

        protected abstract void ProcessToRun();

        protected abstract bool Validate(T request);

        public bool ExecuteResult { get; private set; }
    }
}
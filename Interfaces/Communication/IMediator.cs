using System;

namespace Interfaces
{
    public interface IMediator
    {
        void RegisterInterest(int msgID, Action<object> callback, object caller);

        void InformChange(int msgID, object payload, object caller);
    }
}
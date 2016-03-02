using System;

namespace Interfaces
{
    public interface IIOCContainer
    {
        object GetInstance(Type type);
        object GetInstance(string type);

        object GetSingleInstance(string type);    
    }
}

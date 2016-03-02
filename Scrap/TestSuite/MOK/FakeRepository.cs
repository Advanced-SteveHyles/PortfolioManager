using Interfaces;
using System;

namespace TestSuite.MOK
{
   public class FakeRepository : IIOCContainer 
    {
        public object GetInstance(Type type)
        {
            throw new NotImplementedException();
        }

        public object GetSingleInstance(string type)
        {
            throw new NotImplementedException();
        }


        public object GetInstance(string type)
        {
            throw new NotImplementedException();
        }
    }
}

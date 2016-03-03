using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICommandRunner
    {
        void Execute();

        bool CommandValid { get; }
        bool ExecuteResult { get; }
    }
}

namespace Interfaces
{
    public interface IProcess
    {
        void Execute();

        bool ProcessValid { get; }
        bool ExecuteResult { get; }
    }
}

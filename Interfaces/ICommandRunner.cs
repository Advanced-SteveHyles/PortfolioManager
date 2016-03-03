namespace Interfaces
{
    public interface ICommandRunner
    {
        void Execute();

        bool CommandValid { get; }
        bool ExecuteResult { get; }
    }
}

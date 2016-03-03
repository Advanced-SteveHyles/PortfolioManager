using System;
using System.IO;

namespace Portfolio.API.WebApi.Controllers.Transactions
{
    internal class Command
    {
        
        internal static bool ExecuteCommand(ICommandRunner command)
        {
            try
            {
                if (!command.CommandValid)
                {
                    ErrorLog.LogError(new  InvalidDataException());
                    return false;
                }

                command.Execute();
                return command.ExecuteResult;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return false;
            }            
        }
    }   
}
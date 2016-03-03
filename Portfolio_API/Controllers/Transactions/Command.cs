using System;
using System.IO;
using Interfaces;

namespace Portfolio_API.Controllers.Transactions
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
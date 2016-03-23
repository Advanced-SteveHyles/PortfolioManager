using System;
using System.IO;
using Interfaces;

namespace Portfolio.API.WebApi.Controllers.Transactions
{
    internal class Command
    {
        
        internal static bool ExecuteCommand(IProcess command)
        {
            try
            {
                if (!command.ProcessValid)
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
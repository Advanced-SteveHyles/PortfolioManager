using System;

namespace VirtualService
{
    public class ErrorLog
    {
        public static void LogError(Exception exception)
        {
            Console.Write(exception.Message);
        }
    }
}
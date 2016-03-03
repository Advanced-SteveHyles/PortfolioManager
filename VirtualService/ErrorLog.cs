using System;

namespace Portfolio.API.Virtual
{
    public class ErrorLog
    {
        public static void LogError(Exception exception)
        {
            Console.Write(exception.Message);
        }
    }
}
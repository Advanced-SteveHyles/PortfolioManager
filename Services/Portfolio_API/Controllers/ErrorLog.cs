using System;

namespace Portfolio_API.Controllers
{
    public class ErrorLog
    {
        public static void LogError(Exception exception)
        {
            Console.Write(exception.Message);
        }
    }
}
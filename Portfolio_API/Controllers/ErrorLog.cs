using System;

namespace Portfolio.API.WebApi.Controllers
{
    public class ErrorLog
    {
        public static void LogError(Exception exception)
        {
            Console.Write(exception.Message);
        }
    }
}
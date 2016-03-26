using System;

namespace Portfolio.BackEnd.BusinessLogic.Validators
{
    public static class Validators
    {
        public static bool IsValidDate(DateTime date)
        {
            return date != DateTime.MinValue;
        }
    }
}
using System;

namespace Portfolio.BackEnd.BusinessLogic.Validators
{
    public static class GlobalValidators
    {
        public static bool IsValidDate(DateTime date)
        {
            return date != DateTime.MinValue;
        }
    }
}
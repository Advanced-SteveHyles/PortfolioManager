using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.BackEnd.BusinessLogic
{
    public static class ExtensionMethods
    {
        public static decimal Negate(this decimal value)
        {
            return -value;
        }
    }
}

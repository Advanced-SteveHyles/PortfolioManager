using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class ShapedData
    {
        public static IQueryable<BackEnd.Repository.Entities.Portfolio> CreateDataShapedObject(BackEnd.Repository.Entities.Portfolio portfolio, List<string> lstOfFields)
        {
            throw new NotImplementedException();
        }
    }

    public class Tracer
    {
        public static void Trace(string value)
        {
            Debug.WriteLine(value);
        }
    }
}
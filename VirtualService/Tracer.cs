using System.Diagnostics;

namespace Portfolio.API.Virtual.VirtualControllers
{
    public class Tracer
    {
        public static void Trace(string value)
        {
            Debug.WriteLine(value);
        }
    }
}
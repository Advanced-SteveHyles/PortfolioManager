using System.Linq;
using PortfolioManager.Repository;

namespace DBCreator
{
    class Program
    {
        static void Main(string[] args)
        {          
            using (var ctx = new PortfolioManagerContext())
            {
                //var x = from x1 in ctx.DBGenerator select x1;

                //ctx.SaveChanges();
                                
            }
            
        }
    }
}

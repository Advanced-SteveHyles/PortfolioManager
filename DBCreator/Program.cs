﻿using Portfolio.BackEnd.Repository;

namespace DBCreator
{
    class Program
    {
        static void Main(string[] args)
        {          
            using (var ctx = new PortfolioManagerContext(string.Empty))
            {
                //var x = from x1 in ctx.DBGenerator select x1;

                //ctx.SaveChanges();
                                
            }
            
        }
    }
}

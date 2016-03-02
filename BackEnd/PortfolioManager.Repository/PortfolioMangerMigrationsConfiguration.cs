using System.Data.Entity.Migrations;
using System.Linq;

namespace PortfolioManager.Repository
{
    public class PortfolioMangerMigrationsConfiguration : DbMigrationsConfiguration<PortfolioManagerContext>
    {
        public PortfolioMangerMigrationsConfiguration()
        {
            this.AutomaticMigrationDataLossAllowed = true;  // Deletes field if not longer used
            this.AutomaticMigrationsEnabled = true; // Allows fixing
        }

        protected override void Seed(PortfolioManagerContext context)
        {
            //Seed called everytime application restarts
            //Allows seeding
            base.Seed(context);

            if (!context.Portfolios.Any())
            {
                //Allow seeding

            }

            //foreach (var ea in Enum.GetValues(typeof(EnumAccountType)))
            //{
            //    EnumToTableFactory.CreateAccountTypeTable(context, (EnumAccountType)ea);
            //}

            //foreach (var ea in Enum.GetValues(typeof(EnumTransactionType)))
            //{
            //    EnumToTableFactory.CreateTransactionTypeTable(context, (EnumTransactionType)ea);
            //}

        }
    }
}

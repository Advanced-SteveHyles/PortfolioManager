
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using PortfolioManager.Repository.Entities;

namespace PortfolioManager.Repository
{
    public class PortfolioManagerContext : DbContext
    {
        public PortfolioManagerContext(string connection) : base(connection)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<PortfolioManagerContext>(new NullDatabaseInitializer<PortfolioManagerContext>());
            //    Database.SetInitializer<PortfolioManagerContext>(new DropCreateDatabaseAlways<PortfolioManagerContext>());
        }

        public DbSet<DBGenerator> DBGenerator { get; set; }

        public virtual DbSet<Portfolio> Portfolios { get; set; }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Investment> Investments { get; set; }
        public virtual DbSet<CashTransaction> CashTransactions { get; set; }
        public virtual DbSet<FundTransaction> FundTransactions { get; set; }
        public virtual DbSet<AccountInvestmentMap> AccountInvestmentMaps { get; set; }
        public virtual DbSet<PriceHistory> PriceHistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Portfolio>()
                .HasMany(e => e.Accounts);
        }
    }

    public class DBGenerator
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VersionID { get; set; }
    }
}
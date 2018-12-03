using System.Data.Entity;
namespace TTG.Core
{
    public class TTGContext:DbContext
    {
        public DbSet<Administrator> Administrators { set; get; }
        public DbSet<Role> Roles { set; get; }
        public DbSet<User> Users { get; set; }
        public DbSet<VirtualCurrency> VirtualCurrencies { get; set; }
        public DbSet<Entrust> Entrusts { get; set; }
        public DbSet<PriceTable> PriceTable { get; set; }
        public DbSet<Type> Types { get; set; }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<PriceInDay> PriceInDay { get; set; }
        public DbSet<EntrustDetails> EntrustsDetails { get; set; }
      
        public DbSet<PriceInADeal> PriceInADeals { get; set; }
        public DbSet<UserIdenty> UserIdenties { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public TTGContext() : base("DefaultConnection")
        {
            Database.SetInitializer<TTGContext>(new CreateDatabaseIfNotExists<TTGContext>());
        }
    }

}

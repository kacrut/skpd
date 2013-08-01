using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using skpd.Models.Mapping;

namespace skpd.Models
{
    public partial class ESKAPEDEContext : DbContext
    {
        static ESKAPEDEContext()
        {
            Database.SetInitializer<ESKAPEDEContext>(null);
        }

        public ESKAPEDEContext()
            : base("Name=ESKAPEDEContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<webpages_Membership> webpages_Membership { get; set; }
        public DbSet<webpages_Roles> webpages_Roles { get; set; }
        public DbSet<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryArea> CountryAreas { get; set; }
        public DbSet<CountryInRegion> CountryInRegions { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<LevelPosition> LevelPositions { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PositionType> PositionTypes { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<CashDaily> CashDailies { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Flag> Flags { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestInProgram> RequestInPrograms { get; set; }
        public DbSet<RequestInTransport> RequestInTransports { get; set; }
        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<RequestRelease> RequestReleases { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<EyeBudget> EyeBudgets { get; set; }
        public DbSet<ReportRequest> ReportRequests { get; set; }
        public DbSet<vwPositionInProgram> vwPositionInPrograms { get; set; }
        public DbSet<vwRequest> vwRequests { get; set; }
        public DbSet<vwUserProfile> vwUserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new webpages_MembershipMap());
            modelBuilder.Configurations.Add(new webpages_RolesMap());
            modelBuilder.Configurations.Add(new webpages_UsersInRolesMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new CountryAreaMap());
            modelBuilder.Configurations.Add(new CountryInRegionMap());
            modelBuilder.Configurations.Add(new DivisionMap());
            modelBuilder.Configurations.Add(new LevelPositionMap());
            modelBuilder.Configurations.Add(new PositionMap());
            modelBuilder.Configurations.Add(new PositionTypeMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new UnitMap());
            modelBuilder.Configurations.Add(new CashDailyMap());
            modelBuilder.Configurations.Add(new CurrencyMap());
            modelBuilder.Configurations.Add(new ExchangeRateMap());
            modelBuilder.Configurations.Add(new FlagMap());
            modelBuilder.Configurations.Add(new ProgramMap());
            modelBuilder.Configurations.Add(new RequestMap());
            modelBuilder.Configurations.Add(new RequestInProgramMap());
            modelBuilder.Configurations.Add(new RequestInTransportMap());
            modelBuilder.Configurations.Add(new RequestLogMap());
            modelBuilder.Configurations.Add(new RequestReleaseMap());
            modelBuilder.Configurations.Add(new TransportMap());
            modelBuilder.Configurations.Add(new EyeBudgetMap());
            modelBuilder.Configurations.Add(new ReportRequestMap());
            modelBuilder.Configurations.Add(new vwPositionInProgramMap());
            modelBuilder.Configurations.Add(new vwRequestMap());
            modelBuilder.Configurations.Add(new vwUserProfileMap());
        }
    }
}

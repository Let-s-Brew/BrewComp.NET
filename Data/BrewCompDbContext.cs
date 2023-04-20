using BrewCode.AddressTools.Models;
using BrewCode.BrewGuide;
using BrewComp.Areas.Configuration.Models;
using BrewComp.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql;
using System.Text.Json;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BrewComp.Data
{
    public class BrewCompDbContext : IdentityDbContext<BrewCompUser>
    {
        // User/Role DbSets are done by the Base class

        public DbSet<Competition> Competitions { get; set; }
        public DbSet<HomebrewClub> Clubs { get; set; }

        private ServerDBConfig _dbConfig = new ServerDBConfig();

        public BrewCompDbContext(DbContextOptions<BrewCompDbContext> options)
            : base(options)
        {
            Program.Tracker.Track(_dbConfig);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var npgb = new NpgsqlConnectionStringBuilder();
            npgb.Host = _dbConfig.DBHost;
            npgb.Port = _dbConfig.DBPort;
            npgb.Username = _dbConfig.DBUser;
            npgb.Password = _dbConfig.DBPass;
            npgb.Database = _dbConfig.DBName;
            npgb.TrustServerCertificate = true;
            //npgb.Add("trusted_connection", true);
            optionsBuilder.UseNpgsql(npgb.ConnectionString, o => o.UseNodaTime());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<CivicAddress>().HaveConversion<JSONSerializer<CivicAddress>>();
            configurationBuilder.Properties<List<CivicAddress>>().HaveConversion<JSONSerializer<List<CivicAddress>>>();
            configurationBuilder.Properties<Interval>().HaveConversion<JSONSerializer<Interval>>();
            configurationBuilder.Properties<IGuidelines<IStyleCategory<IStyle>, IStyle>>().HaveConversion<GuidelinesConverter>();
            configurationBuilder.Properties<IStyle>().HaveConversion<StyleConverter>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BrewCompUser>(bb =>
            {
                bb.ToTable(name: "Users");
                bb.HasIndex(u => u.Id).IncludeProperties(u => new { u.NormalizedUserName, u.LastName, u.FirstName });
                bb.HasMany(b => b.Entries);
                bb.HasMany(u => u.Competitions);
                bb.HasOne(u => u.Club); // Do we want to allow entrants to be able to belong to more than one club?

            });

            builder.Entity<CompetitionEntry>(ceb =>
            {
                ceb.ToTable(name: "Entries");
                ceb.HasIndex(e => e.Id).IncludeProperties(e => e.EntryId);
                ceb.HasOne(ce => ce.Brewer);
                ceb.HasOne(ce => ce.Competition);
            });

            builder.Entity<Competition>(cb =>
            {
                cb.ToTable(name: "Competitions");
                cb.HasIndex(e => e.Id).IncludeProperties(e => e.Name);
                cb.HasMany(c => c.Entries).WithOne().HasForeignKey(e => e.Id);
                cb.HasMany(c => c.Entrants).WithMany(e => e.Competitions);
            });


            builder.HasDefaultSchema("Identity");

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

        }
    }

    internal class GuidelinesConverter : ValueConverter<IGuidelines<IStyleCategory<IStyle>, IStyle>, string>
    {

        public GuidelinesConverter()
            : base( // TODO - serialize other guideline types to an ID. 
                  g => "2021BJCP",
                  s => (IGuidelines<IStyleCategory<IStyle>, IStyle>)BrewCode.BrewGuide.BJCP.Guidelines.BJCP2021Guidelines
             )
        { }
    }

    internal class StyleConverter : ValueConverter<IStyle, string>
    {
        public StyleConverter()
            : base(
                s => s.Id,
                s => BrewCode.BrewGuide.BJCP.Guidelines.StyleFromString(s)
        )
        { }
    }

    internal class JSONSerializer<T> : ValueConverter<T, string>
    {
        public JSONSerializer()
            : base(
                  o => JsonSerializer.Serialize(o, new JsonSerializerOptions()),
                  j => JsonSerializer.Deserialize<T>(j, new JsonSerializerOptions())
        )
        { }
    }
}
using BrewCode.AddressTools.Models;
using BrewCode.BrewGuide;
using BrewComp.Areas.Configuration.Models;
using BrewComp.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;

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

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<CivicAddress>().HaveConversion<JSONSerializer<CivicAddress>>();
            configurationBuilder.Properties<List<CivicAddress>>().HaveConversion<JSONSerializer<List<CivicAddress>>, ListComparer<CivicAddress>>();
            configurationBuilder.Properties<Interval>().HaveConversion<JSONSerializer<Interval>>();
            configurationBuilder.Properties<IGuidelines<IStyleCategory<IStyle>, IStyle>>().HaveConversion<GuidelinesConverter>();
            configurationBuilder.Properties<IStyle>().HaveConversion<StyleConverter>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("BrewComp");

            builder.Entity<BrewCompUser>(u =>
            {
                u.ToTable(name: "Users");
                u.HasIndex(u => u.Id).IncludeProperties(u => new { u.NormalizedUserName, u.LastName, u.FirstName });
                u.HasMany(b => b.Entries).WithOne(e => e.Brewer);
                u.HasMany(u => u.Competitions).WithMany(c => c.Entrants);
                u.HasOne(u => u.Club); // Do we want to allow entrants to be able to belong to more than one club?

            });

            builder.Entity<Competition>(cb =>
            {
                cb.ToTable(name: "Competitions");
                cb.HasIndex(e => e.Id).IncludeProperties(e => e.Name);
                cb.HasMany(c => c.Entries).WithOne(e => e.Competition);
                cb.HasMany(c => c.Entrants).WithMany(e => e.Competitions);
                cb.HasOne(c => c.Host).WithMany().IsRequired();
            });

            builder.Entity<CompetitionEntry>(ceb =>
            {
                ceb.ToTable(name: "Entries");
                ceb.HasIndex(e => e.Id).IncludeProperties(e => e.EntryId);
            });

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

    internal class ListComparer<T> : ValueComparer<List<T>>
    {
        public ListComparer()
            : base(
                  (l1, l2) => l1.Count == l2.Count && l1.All(x => l2.Contains(x)),
                  l => l.GetHashCode(),
                  l => l.ToList()
            )
        { }
    }
}
using BrewComp.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BrewComp.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserIdentity>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var intervalSerializer = new ValueConverter<Interval, string>(
                i => JsonSerializer.Serialize(i, new JsonSerializerOptions()),
                s => JsonSerializer.Deserialize<Interval>(s, new JsonSerializerOptions())
                );

            builder.Entity<Competition>().Property(v => v.CompetitionDates).HasConversion(intervalSerializer);
            builder.Entity<Competition>().Property(v => v.ShippingDates).HasConversion(intervalSerializer);
            builder.Entity<Competition>().Property(v => v.DropOffDates).HasConversion(intervalSerializer);
            builder.Entity<Competition>().Property(v => v.RegistrationDates).HasConversion(intervalSerializer);
            builder.Entity<Competition>().Property(v => v.EntryRegistrationDates).HasConversion(intervalSerializer);

            builder.HasDefaultSchema("Identity");
            
            builder.Entity<UserIdentity>(entity =>
            {
                entity.ToTable(name: "User");
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
}
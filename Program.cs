using BrewComp.Areas.Identity;
using BrewComp.Data;
using BrewComp.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace BrewComp;
public class Program
{
    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<BrewCompDbContext>(o => o.UseNpgsql(connStr, o=>o.UseNodaTime()));
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddIdentity<BrewCompUser, IdentityRole>(o => o.SignIn.RequireConfirmedEmail = false)
            .AddDefaultTokenProviders()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<BrewCompDbContext>();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<BrewCompUser>>();
        builder.Services.AddSingleton<IEmailSender, EmailSender>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
        });

        app.MapRazorPages();
        app.MapControllers();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        //Default Roles
        using (var scope = app.Services.CreateScope())
        {
            var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] _defaultRoles = { "siteadmin", "coordinator", "participant", "steward", "judge" };

            foreach (var role in _defaultRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var _context = scope.ServiceProvider.GetRequiredService<BrewCompDbContext>();
            
            if(_context.Clubs.Count() == 0)
            {
                HomebrewClub.PopulateDb(_context, app.Logger);
            }
        }
        app.Run();
        // This thread is now blocked, won't exit until app is closed/shutdown

    }
}
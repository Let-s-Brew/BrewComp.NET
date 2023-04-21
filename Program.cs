using BrewComp.Areas.Configuration.Models;
using BrewComp.Areas.Identity;
using BrewComp.Data;
using BrewComp.Identity;
using Jot;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BrewComp;
public class Program
{
    internal static Tracker Tracker = new Tracker();
    public static async Task Main(string[] args)
    {
        //Use JOT to save/persist the DB info
        Tracker.Configure<ServerDBConfig>().Properties(
                config => new
                {
                    //config.DBType, //TODO Support multiple DB implementations?
                    config.DBName,
                    config.DBHost,
                    config.DBPort,
                    config.DBUser,
                    config.DBPass
                }
            );

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<BrewCompDbContext>();
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
        }
        app.Run();

        // This thread is now blocked, won't exit until app is closed/shutdown
        Tracker.PersistAll();
    }
}
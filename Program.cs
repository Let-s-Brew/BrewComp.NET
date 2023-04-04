using BrewComp.Areas.Configuration.Models;
using BrewComp.Areas.Identity;
using BrewComp.Data;
using BrewComp.Identity;
using Jot;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;


namespace BrewComp;  
public class Program
{
    internal static Tracker Tracker = new Tracker();
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Use JOT to save/persist the DB info
        Tracker.Configure<ServerDBConfig>().Properties(
                config => new {
                    //config.DBType, //TODO Support multiple DB implementations?
                    config.DBName,
                    config.DBHost,
                    config.DBPort,
                    config.DBUser,
                    config.DBPass
                }
            );
        var config = new ServerDBConfig();

        //DB Setup
        var npgb = new NpgsqlConnectionStringBuilder();
        npgb.Host = config.DBHost;
        npgb.Port = config.DBPort;
        npgb.Username = config.DBUser;
        npgb.Password = config.DBPass;
        npgb.Database = config.DBName;
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>      
            opt.UseNpgsql(npgb.ConnectionString, o => o.UseNodaTime()));
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddDefaultIdentity<UserIdentity>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<UserIdentity>>();

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

        //Default Roles
        var _roleManager = app.Services.GetRequiredService<RoleManager<IdentityRole>>();
        string[] _defaultRoles = { "admin", "coordinator", "participant", "steward", "judge" };

        foreach( var role in _defaultRoles )
        {
            if(!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        app.MapControllers();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();

        // This thread is now blocked, won't exit until app is closed/shutdown
        Tracker.PersistAll();
    }
}
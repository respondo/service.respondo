using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respondo.Core.Identity.Contracts.Entities;
using Respondo.Core.Identity.Persistence;
using Wolverine;

namespace Respondo.Core.Identity.Configuration;

/// <summary>
///     Extensions for configuring the Identity Core Module
/// </summary>
public static class CoreExtensions
{
    /// <summary>
    ///     Configure the Identity Core Module, including DbContext, DataProtection, etc.     
    /// </summary>
    /// <param name="builder"><see cref="WebApplicationBuilder"/>.</param>
    public static void ConfigureIdentityModule(this WebApplicationBuilder builder)
    {
        var identityDbConnectionString = builder.Configuration.GetConnectionString("IdentityDb");

        builder.Services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseNpgsql(identityDbConnectionString, optionsBuilder => { });
        });

        builder.Services.AddDataProtection().PersistKeysToDbContext<IdentityDbContext>();

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 12;
            }).AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.Cookie.IsEssential = true;
        });
        builder.Services.AddAuthorization();
    }

    /// <summary>
    ///     Run migrations for <see cref="IdentityDbContext"/>.
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/>.</param>
    public static void RunIdentityDbMigrations(this WebApplication app)
    {
        using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
        serviceScope?.ServiceProvider.GetRequiredService<IdentityDbContext>().Database.Migrate();
    }
    
    /// <summary>
    ///     Include Wolverine handlers for Identity Core operations.
    /// </summary>
    /// <param name="options"><see cref="WolverineOptions"/>.</param>
    /// <param name="configuration"><see cref="IConfiguration"/>.</param>
    public static void IncludeIdentityCore(this WolverineOptions options, IConfiguration configuration)
    {
        options.Discovery.IncludeType<CreateApplicationUserHandler>();
    }
}
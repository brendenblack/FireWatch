using Firewatch.Application.Common.Interfaces;
using Firewatch.Infrastructure.Files;
using Firewatch.Infrastructure.Identity;
using Firewatch.Infrastructure.Persistence;
using Firewatch.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Firewatch.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("FirewatchDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            //services.AddIdentityServer()
            //    .AddAspNetIdentity<ApplicationUser>();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration.GetValue<string>("TokenAuthority");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScpe", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1");
                });
            });


            //services.AddDefaultIdentity<ApplicationUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
            services.AddTransient<ITradeParserService, TradeLogTradeParserService>();

            //services.AddAuthentication()
            //    .AddIdentityServerJwt();

            return services;
        }
    }
}

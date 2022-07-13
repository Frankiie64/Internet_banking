using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Infrastructure.Identity.Contexts;
using Internet_banking.Infrastructure.Identity.Entities;
using Internet_banking.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Infrastructure.Identity
{
    public static class ServicesRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Configuration of Database
            if (configuration.GetValue<bool>("UseInMemoryDataBase"))
            {
                services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("DBInternetBakingIdentity"));
            }
            else
            {

                services.AddDbContext<IdentityContext>(Options =>
                Options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionIdentity"),
                m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            }
            #endregion

            #region Repositories

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication();
            #endregion

            #region Identity

            services.AddTransient<IAccountServices, AccountServices>();

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/User";
                opt.AccessDeniedPath = "/User/AccessDenied";
            });

            #endregion

            services.AddAuthentication();
        }
    }

}

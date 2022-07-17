using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.Interfaces.Repositories;
using Internet_banking.Core.Application.Seeds;
using Internet_banking.Core.Application.ViewModels.Users;
using Internet_banking.Infrastructure.Identity.Entities;
using Internet_banking.Infrastructure.Identity.Seeds;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppl.Internet_banking
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           var runApp = CreateHostBuilder(args).Build();
            using (var scope = runApp.Services.CreateScope())
            {               
                var services = scope.ServiceProvider;
                try
                {                   
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var productRepository = services.GetRequiredService<ITypeAccountRepository>();
                    var TrasationalRepository = services.GetRequiredService<ITransactionRepository>();


                    await DefaultRoles.SeedAsync(userManager, roleManager);
                    await DefaultBasicUser.SeedAsync(userManager, roleManager);
                    await DefaultAdminUser.SeedAsync(userManager,roleManager);
                    await DefaultSuperAdminUser.SeedAsync(userManager, roleManager);

                    // Default accounts created behind the program star cause is necesary there are created before their functionality
                    await DefaultPrincipalAccount.SeedAsync(productRepository);
                    await DefaultSavingAccount.SeedAsync(productRepository);
                    await DefaultCreditCard.SeedAsync(productRepository);
                    await DefaultDebt.SeedAsync(productRepository);
                    await DefaultTransantionsTable.SeedAsync(TrasationalRepository);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            runApp.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

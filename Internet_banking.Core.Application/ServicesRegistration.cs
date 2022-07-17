using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            #region AutoMapper

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #endregion
            #region Services 

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITypeAccountService, TypeAccountService>();
            services.AddTransient<IProductServices, ProductServices>();
            services.AddTransient<IBeneficiaryServices, BeneficiaryServices>();
            services.AddTransient<ITrasantionalService, TrasantionalService>();

            #endregion
        }
    }
}

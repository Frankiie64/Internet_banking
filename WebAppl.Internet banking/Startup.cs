using Internet_banking.Core.Application;
using Internet_banking.Infrastructure.Identity;
using Internet_banking.Infrastructure.Persistence;
using Internet_banking.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppl.Internet_banking.middlewares;

namespace WebAppl.Internet_banking
{
    public class Startup
    {
        public IConfiguration _Config { get; }

        public Startup(IConfiguration Config)
        {
            _Config = Config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddPersitsenceInfrastructure(_Config);
            services.AddIdentityInfrastructure(_Config);
            services.AddApplicationLayer();
            services.AddSharedInfrastructure(_Config);
            services.AddControllersWithViews();
            services.AddScoped<LoginAuthorize>();
            services.AddScoped<SelectHome>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ValidSession, ValidSession>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Index}/{id?}");
            });
        }
    }
}

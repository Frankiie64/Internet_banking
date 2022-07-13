using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppl.Internet_banking.Controllers;

namespace WebAppl.Internet_banking.middlewares
{
    public class SelectHome: IAsyncActionFilter
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly AuthenticationResponse user;
        public SelectHome(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
            user = httpContext.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (user.Roles.Count > 1)
            {
                await next();
            }

            if (user.Roles.Any(r => r == "Basic"))
            {
                var controller = (HomeController)context.Controller;
                context.Result = controller.RedirectToAction("IndexClient", "home");
            }

            if (user.Roles.Any(r=>r == "Admin"))
            {
                var controller = (HomeController)context.Controller;
                context.Result = controller.RedirectToAction("IndexAdmin", "home");
            }            
        }
    }
}


using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppl.Internet_banking.Controllers;

namespace WebAppl.Internet_banking.middlewares
{
    public class LoginAuthorize: IAsyncActionFilter
    {
        private readonly ValidSession _userSession;
        public LoginAuthorize(ValidSession userSession)
        {
            _userSession = userSession;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_userSession.HasUser())
            {
                var controller = (UserController)context.Controller;
                context.Result = controller.RedirectToAction("index", "home");
            }
            else
            {
                await next();
            }
        }
    }
}

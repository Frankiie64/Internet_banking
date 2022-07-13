using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.helper;
using Internet_banking.Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppl.Internet_banking.middlewares
{
    public class ValidSession
    {
        private readonly IHttpContextAccessor _context;

        public ValidSession(IHttpContextAccessor context)
        {
            _context = context;
        }
        public bool HasUser()
        {
            AuthenticationResponse user = _context.HttpContext.Session.Get<AuthenticationResponse>("user");

            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}

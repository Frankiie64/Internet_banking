using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebAppl.Internet_banking.middlewares;
using WebAppl.Internet_banking.Models;

namespace WebAppl.Internet_banking.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService userService;
        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        [ServiceFilter(typeof(SelectHome))]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> IndexAdmin()
        {
            return View("IndexAdmin", await userService.GetAllUsersAsync());
        }
        [Authorize(Roles ="Basic")]
        public IActionResult IndexClient()
        {
            return View();
        }        

    }
}

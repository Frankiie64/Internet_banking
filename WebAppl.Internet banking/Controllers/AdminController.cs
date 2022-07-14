using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.helper;
using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppl.Internet_banking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService userService;
        public AdminController(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await userService.GetAllUsersAsync());
        }

        public IActionResult CreateAdmin()
        {
            return View(new SaveUserVM());
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(SaveUserVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var origin = Request.Headers["origin"];
            RegisterResponse response = await userService.RegisterAsync(vm, origin);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "Admin", action = "Index" });

        }
        public IActionResult CreateClient()
        {
            return View(new SaveUserVM());
        }
        [HttpPost]
        public IActionResult CreateClient(SaveUserVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var origin = Request.Headers["origin"];

            SingletonRepository.Instance.client = vm;
            SingletonRepository.Instance.origin = origin;

            return RedirectToRoute(new { controller = "Product", action = "CreateClientWithProduct", id = string.Empty, value = false });
        }

        public async Task<IActionResult> IsVerified(string id)
        {
            await userService.IsVerified(id);

            return RedirectToRoute
                (new { controller = "Admin", action = "Index" });
        }

    }
}

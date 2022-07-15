using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.helper;
using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Core.Application.ViewModels.Products;
using Internet_banking.Core.Application.ViewModels.Users;
using Internet_banking.Core.Application.ViewModels.Users.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IProductServices productServices;
        private readonly ITypeAccountService typeAccountService;
        private readonly IHttpContextAccessor context;
        AuthenticationResponse user;

        public AdminController(IUserService userService,IProductServices productServices,ITypeAccountService typeAccountService, IHttpContextAccessor context)
        {
            this.userService = userService;
            this.context = context;
            this.productServices = productServices;
            this.typeAccountService = typeAccountService;
             user = context.HttpContext.Session.Get<AuthenticationResponse>("user");
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
        public async Task<IActionResult> UpdateAdmin(string id)
        {
            if (id == user.Id)
            {
                return RedirectToRoute(new { controller = "User", action = "AccessDenied" });
            }
            var item = await userService.GetSaveUserVMByIdAsync(id);
            return View("CreateAdmin", item);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAdmin(SaveUserVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateAdmin", vm);
            }

            RegisterResponse response = await userService.UpdateUserAsync(vm);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("CreateAdmin", vm);
            }
            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }
        public IActionResult CreateClient()
        {
            return View(new SaveClienteVM());
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(SaveClienteVM vm)
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

            var list = await typeAccountService.GetAllViewModelAsync();

            SaveProductVM productVM = new SaveProductVM();

            productVM.IdAccount = list.FirstOrDefault(item => item.Title == "Cuenta Principal").Id;
            productVM.Amount = vm.amount;
            productVM.IdClient = response.IdClient;

            productVM = await productServices.CreateAsync(productVM);

            if (productVM.Id == 0 || productVM == null)
            {
                vm.HasError = true;
                vm.Error = "Ocurrio un problema creando al producto de la cuenta";
            }

            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }

        public async Task<IActionResult> IsVerified(string id)
        {
            if (id == user.Id)
            {
                return RedirectToRoute(new { controller = "User", action = "AccessDenied" });
            }
            await userService.IsVerified(id);

            return RedirectToRoute
                (new { controller = "Admin", action = "Index" });
        }

    }
}

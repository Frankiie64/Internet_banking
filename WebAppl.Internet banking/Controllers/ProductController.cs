using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.Enums;
using Internet_banking.Core.Application.helper;
using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Core.Application.ViewModels.Products;
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
    public class ProductController : Controller
    {
        private readonly IProductServices services;
        private readonly IUserService servicesUser;
        private readonly ITypeAccountService servicesTypeAccount;

        public ProductController(IProductServices services, IUserService servicesUser, ITypeAccountService servicesTypeAccount)
        {
            this.services = services;
            this.servicesUser = servicesUser;
            this.servicesTypeAccount = servicesTypeAccount;
        }

        public async Task<IActionResult> Index()
        {
            return View(await servicesUser.GetAllClientsAsync());
        }
        public async Task<IActionResult> AddProduct(SaveProductVM vm)
        {            
               var item = await servicesUser.GetUserByIdAsync(vm.IdClient);

                if (item.Roles[0] != Roles.Basic.ToString())
                {
                    return RedirectToRoute(new { controller = "User", action = "AccessDenied" });
                }

            return View("AddProduct", new SaveProductVM() { IdClient = vm.IdClient,IdAccount = vm.IdAccount });
            
        }
        [HttpPost]
        public async Task<IActionResult> AddProductPost(SaveProductVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View("AddProduct", vm);
            }

             await services.CreateAsync(vm);

            return RedirectToRoute(new { controller = "Product", action = "Index" });
        }

    }
}

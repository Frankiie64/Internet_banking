﻿using Internet_banking.Core.Application.Dtos.Account;
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

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateClientWithProduct(string id, bool value = true)
        {
            if (value)
            {
               var item = await servicesUser.GetUserByIdAsync(id);

                if (item.Roles[0] != Roles.Basic.ToString())
                {
                    return RedirectToRoute(new { controller = "User", action = "AccessDenied" });
                }

                ViewData["user"] = item;
                return View("AddProduct", new SaveProductVM() { IdClient = item.Id });
            }

            ViewBag.typesAccount = servicesTypeAccount.GetAllViewModelAsync();

            return View("AddProduct", new SaveProductVM());

        }
        [HttpPost]
        public async Task<IActionResult> CreateClientWithProduct(SaveProductVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (string.IsNullOrWhiteSpace(vm.IdClient))
            {
               var item = await CreateClientWithProduct();

                if (item.HasError)
                {
                    return RedirectToRoute(new { controller = "Product", action = "Error"/*crea una vista en caso de que el usuario se cree mal*/ });
                }
                else
                {
                    vm.IdClient = item.IdClient;
                }
            }
            
            await services.CreateAsync(vm);

            return RedirectToRoute(new { controller = "Product", action = "Index" });
        }
        private async Task<RegisterResponse> CreateClientWithProduct()
        {
            SaveUserVM vm = SingletonRepository.Instance.client;
            string origin = SingletonRepository.Instance.origin;

            RegisterResponse response = await servicesUser.RegisterAsync(vm, origin);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;

                return response;
            }
            return response;
        }
    }
}

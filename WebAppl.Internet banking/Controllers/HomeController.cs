using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Core.Application.ViewModels.Products;
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
        private readonly IProductServices _productServices;
        public HomeController(IUserService userService, IProductServices productServices)
        {
            this.userService = userService;
            _productServices = productServices;
        }

        [ServiceFilter(typeof(SelectHome))]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public IActionResult IndexAdmin()
        {
            return View("IndexAdmin");
        }

        [Authorize(Roles ="Basic")]
        public  IActionResult IndexClient()
        {
            return View();
            //List<ProductsVM> list = await _productServices.GetAllViewModelAsync();

        }

    }
}

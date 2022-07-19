using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.helper;
using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Core.Application.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly ITrasantionalService service;
        private readonly IProductServices Productservice;
        private readonly IHttpContextAccessor context;
        AuthenticationResponse user;

        public HomeController(ITrasantionalService service, IProductServices Productservice, IHttpContextAccessor context)
        {
            this.service = service;
            this.Productservice = Productservice;
            this.context = context;
            user = context.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        [ServiceFilter(typeof(SelectHome))]
        public IActionResult Index()
        {
         
            return View();
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> IndexAdmin()
        {
           
            var items = await service.GetTransationalToday();
            return View("IndexAdmin",items);
        }

        [Authorize(Roles ="Basic")]
        public async Task<IActionResult> IndexClient()
        {
         
            var item = await Productservice.GetAllWithIncludeAsync();
            item = item.Where(pr => pr.IdClient == user.Id).ToList();
            return View("IndexClient",item);
        }

    }
}

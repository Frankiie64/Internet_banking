using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.Enums;
using Internet_banking.Core.Application.helper;
using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Core.Application.ViewModels.Clients.Beneficiary;
using Internet_banking.Core.Application.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppl.Internet_banking.Controllers
{

    [Authorize(Roles = "Basic")]
    public class BeneficiaryController : Controller
    {
        private readonly IBeneficiaryServices _beneficiaryServices;
        private readonly IUserService userService;
        private readonly IProductServices _productServices;

        private readonly IHttpContextAccessor _context;
        AuthenticationResponse user;


        public BeneficiaryController(IUserService userService, IBeneficiaryServices beneficiaryServices, IProductServices productServices, IHttpContextAccessor context)
        {
            this.userService = userService;
            _context = context;
            _beneficiaryServices = beneficiaryServices;
            _productServices = productServices;
            user = context.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<IActionResult> Index(int Id)
        {
            var item = await _beneficiaryServices.GetAllViewModelAsync();
            item = item.Where(x => x.UserId == user.Id).ToList();

            ViewBag.BeneficiaryList = item;

            return View(new SaveBeneficiaryVM());
        }


        [HttpPost]
        public async Task<IActionResult> Index(SaveBeneficiaryVM vm, int id)
        {
            vm.UserId = user.Id;
            var item = await _beneficiaryServices.GetAllViewModelAsync();
            var items= await _productServices.GetAllViewModelAsync();

            var SaveAccount = items.Where(pr => pr.Code == vm.BeneficiaryCode).SingleOrDefault();

          
            item = item.Where(x => x.UserId == user.Id).ToList();

            ViewBag.BeneficiaryList = item;


            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", new SaveBeneficiaryVM());
            }

            if (!await _productServices.Exist(vm.BeneficiaryCode))
            {
                ModelState.AddModelError("", $"El Numero de Cuenta {vm.BeneficiaryCode} no existe.");
                return View("Index", vm);
            }

            if (SaveAccount.IdAccount == (int)TypesAccountEnum.Prestamo || SaveAccount.IdAccount == (int)TypesAccountEnum.Tarjetadecredito)
            {
                ModelState.AddModelError("", $"El Numero de Cuenta {vm.BeneficiaryCode} no es una cuenta de ahorro, favor de ingresar una que si lo sea.");
                return View("Index", vm);
            }

            var beneficiaryRepet = item.Any(x => x.BeneficiaryCode == vm.BeneficiaryCode);

            if (beneficiaryRepet)
            {
                ModelState.AddModelError("", "Ya tienes agregado a este beneficiario");
                return View("Index", vm);
            }

            if (true)
            {

            }

            SaveBeneficiaryVM beneficiaryVM = await _beneficiaryServices.CreateAsync(vm);

            return RedirectToRoute(new { controller = "Beneficiary", action = "Index" });
        }



        public async Task<IActionResult> Delete(int id)
        {
            return View(await _beneficiaryServices.GetByIdSAsync(id));

        }

        [HttpPost]
        public async Task<IActionResult> DeleteBeneficiary(SaveBeneficiaryVM vm)
        {
            
            await _beneficiaryServices.DeleteAsync(vm.Id);

            return RedirectToRoute(new { controller = "Beneficiary", action = "Index" });
        }

    }
}

using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.helper;
using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Core.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internet_banking.Core.Application.ViewModels.Money_Advance;

namespace WebAppl.Internet_banking.Controllers
{
    [Authorize(Roles = "Basic")]

    public class ClientController : Controller
    {
        private readonly IProductServices ProductService;
        private readonly IUserService userService;
        private readonly ITrasantionalService TrasantationService;
        private readonly IHttpContextAccessor _context;
        AuthenticationResponse user;

        public ClientController(IUserService userService, IHttpContextAccessor context, IProductServices ProductService, ITrasantionalService TrasantationService)
        {
            this.userService = userService;
            this.ProductService = ProductService;
            this.TrasantationService = TrasantationService;
            _context = context;
            user = context.HttpContext.Session.Get<AuthenticationResponse>("user");
        }
        public async Task<IActionResult> SaveMoney()
        {
            var items = await ProductService.GetAllViewModelAsync();
            items = items.Where(client => client.IdClient == user.Id).ToList();          

            ViewBag.CreditCards = items.Where(client => client.IdAccount == (int)TypesAccountEnum.Tarjetadecredito).ToList();

            return View("SaveMoney", new SaveMoneyVM() { HasError = false});
        }

        [HttpPost]
        public async Task<IActionResult> SaveMoney(SaveMoneyVM vm)
        {
            vm.HasError = false;
            var items = await ProductService.GetAllViewModelAsync();
            items = items.Where(client => client.IdClient == user.Id).ToList();

            ViewBag.CreditCards = items.Where(client => client.IdAccount == (int)TypesAccountEnum.Tarjetadecredito).ToList();

            if (!ModelState.IsValid)
            {
                return View("SaveMoney",vm);                    
            }

            var CreditCard = await ProductService.GetByIdSaveViewModelAsync(vm.IdcreditCard);

            if ((CreditCard.Amount - CreditCard.Paid)- (vm.Amount +(vm.Amount * 0.625)) < 0)
            {
                vm.HasError = true;
                vm.Error = "No posee suficiente dinero en la tarjeta de credito para realizar dicho avance de efectivo.";
                return View("SaveMoney",vm);
            }

            var SaveAccount = await ProductService.GetProductByCode(vm.CodeSaveAccount);

            if (SaveAccount == null)
            {
                vm.HasError = true;
                vm.Error = "La cuenta de ahorro no existe.";
                return View("SaveMoney", vm);
            }
            if (SaveAccount.IdAccount != (int)TypesAccountEnum.Cuentadeahorro && SaveAccount.IdAccount != (int)TypesAccountEnum.CuentaPrincipal)
            {
                vm.HasError = true;
                vm.Error = "Solo esta permito cuentas de ahorros.";
                return View("SaveMoney", vm);
            }

            CreditCard.Paid += (vm.Amount * 0.0625) + vm.Amount;
            bool value = await ProductService.UpdateAsync(CreditCard, vm.IdcreditCard);

            if (!value)
            {
                vm.HasError = true;
                vm.Error = "Ha ocurrido un error intentando hacer la transferencia en la tarjeta de credito, por favor llamar al servicio tecnico.";
                return View("SaveMoney", vm);
            }

            SaveAccount.Amount += vm.Amount;
            value = await ProductService.UpdateAsync(SaveAccount, SaveAccount.Id);

            if (!value)
            {
                CreditCard.Paid -= (vm.Amount * 0.0625) + vm.Amount;
                vm.HasError = true;
                vm.Error = "Ha ocurrido un error intentando hacer el deposito en la cuenta de ahorro, por favor llamar al servicio tecnico y revisar que su no se haya debitado el monto de la tarjeta de credito.";
                return View("SaveMoney", vm);
            }


            var trasantion = await TrasantationService.GetByDateTrasations();

            if (trasantion == null)
            {

                CreditCard.Paid -= (vm.Amount * 0.0625) + vm.Amount;
                await ProductService.UpdateAsync(CreditCard, vm.IdcreditCard);

                SaveAccount.Amount -= vm.Amount;
                await ProductService.UpdateAsync(SaveAccount, SaveAccount.Id);

                vm.HasError = true;
                vm.Error = "Ha ocurrido un error interno, por favor llamar al servicio tecnico .";
                return View("SaveMoney", vm);
            }

            trasantion.Count_transactional += 1;

            value = await TrasantationService.UpdateAsync(trasantion,trasantion.Id);

            if (!value)
            {

                CreditCard.Paid -= (vm.Amount * 0.0625) + vm.Amount;
                await ProductService.UpdateAsync(CreditCard, vm.IdcreditCard);

                SaveAccount.Amount -= vm.Amount;
                await ProductService.UpdateAsync(SaveAccount, SaveAccount.Id);

                vm.HasError = true;
                vm.Error = "Ha ocurrido un error interno, por favor llamar al servicio tecnico .";
                return View("SaveMoney", vm);
            }

            vm = new();
            vm.IdcreditCard = 0;
            vm.CodeSaveAccount = 0;
            vm.Amount = 0;
            vm.HasError = true;
            vm.Error = "La operacion se ha ejecutado de manera sactifactoria, favor revisar ambas cuentas.";
            return View("SaveMoney", vm);
        }

        public async Task<IActionResult> Transferencia()
        {
            var items = await ProductService.GetAllViewModelAsync();
            items = items.Where(client => client.IdClient == user.Id).ToList();

            ViewBag.SaveAccounts = items.Where(client =>
            client.IdAccount == (int)TypesAccountEnum.Cuentadeahorro || 
            client.IdAccount == (int)TypesAccountEnum.CuentaPrincipal).ToList();

            return View("Transferencia", new SaveAccountMoneyVM() { HasError = false });
        }

        [HttpPost]
        public async Task<IActionResult> Transferencia(SaveAccountMoneyVM vm)
        {
            vm.HasError = false;
            var items = await ProductService.GetAllViewModelAsync();
            items = items.Where(client => client.IdClient == user.Id).ToList();

            ViewBag.SaveAccounts = items.Where(client =>
            client.IdAccount == (int)TypesAccountEnum.Cuentadeahorro ||
            client.IdAccount == (int)TypesAccountEnum.CuentaPrincipal).ToList();

            if (!ModelState.IsValid)
            {
                return View("Transferencia", vm);
            }

            var SaveAccount = await ProductService.GetByIdSaveViewModelAsync(vm.IdSaveAccount);

            if ((SaveAccount.Amount  - vm.Amount) < 0)
            {
                vm.HasError = true;
                vm.Error = "No posee suficiente dinero en la cuenta de credito para realizar dicha tranferencia.";
                return View("Transferencia", vm);
            }

            var Destination = await ProductService.GetProductByCode(vm.CodeSaveAccount);

            if (Destination == null)
            {
                vm.HasError = true;
                vm.Error = "La cuenta de ahorro no existe.";
                return View("Transferencia", vm);
            }
            if (SaveAccount.IdAccount != (int)TypesAccountEnum.Cuentadeahorro && SaveAccount.IdAccount != (int)TypesAccountEnum.CuentaPrincipal)
            {
                vm.HasError = true;
                vm.Error = "Solo esta permito cuentas de ahorros.";
                return View("Transferencia", vm);
            }

            SaveAccount.Amount -=  vm.Amount;
            bool value = await ProductService.UpdateAsync(SaveAccount, vm.IdSaveAccount);

            if (!value)
            {
                vm.HasError = true;
                vm.Error = "Ha ocurrido un error intentando hacer la transferencia en la cuenta de origen, por favor llamar al servicio tecnico.";
                return View("Transferencia", vm);
            }

            Destination.Amount += vm.Amount;
            value = await ProductService.UpdateAsync(Destination, Destination.Id);

            if (!value)
            {
                SaveAccount.Amount += vm.Amount;
                vm.HasError = true;
                vm.Error = "Ha ocurrido un error intentando hacer el deposito en la cuenta de ahorro, por favor llamar al servicio tecnico y revisar que su no se haya debitado el monto de la tarjeta de credito.";
                return View("Transferencia", vm);
            }


            var trasantion = await TrasantationService.GetByDateTrasations();

            if (trasantion == null)
            {

                SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(SaveAccount, vm.IdSaveAccount);

                Destination.Amount -= vm.Amount;
                await ProductService.UpdateAsync(SaveAccount, SaveAccount.Id);

                vm.HasError = true;
                vm.Error = "Ha ocurrido un error interno, por favor llamar al servicio tecnico .";
                return View("Transferencia", vm);
            }

            trasantion.Count_transactional += 1;

            value = await TrasantationService.UpdateAsync(trasantion, trasantion.Id);

            if (!value)
            {

                Destination.Amount -= vm.Amount;
                await ProductService.UpdateAsync(Destination, vm.IdSaveAccount);

                SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(SaveAccount, vm.IdSaveAccount);

                vm.HasError = true;
                vm.Error = "Ha ocurrido un error interno, por favor llamar al servicio tecnico .";
                return View("Transferencia", vm);
            }

            vm = new();
            vm.IdSaveAccount = 0;
            vm.CodeSaveAccount = 0;
            vm.Amount = 0;
            vm.HasError = true;
            vm.Error = "La operacion se ha ejecutado de manera sactifactoria, favor revisar ambas cuentas.";
            return View("Transferencia", vm);
        }


    }
}

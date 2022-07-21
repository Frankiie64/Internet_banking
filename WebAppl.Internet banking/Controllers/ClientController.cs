using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.ViewModels.Products;
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
using Internet_banking.Core.Application.ViewModels.Clients.Paids;
using Internet_banking.Core.Application.ViewModels.Express_Paid;

namespace WebAppl.Internet_banking.Controllers
{
    [Authorize(Roles = "Basic")]

    public class ClientController : Controller
    {
        private readonly IProductServices ProductService;
        private readonly IUserService userService;
        private readonly IBeneficiaryServices beneficiaryServices;
        private readonly ITrasantionalService TrasantationService;
        private readonly IHttpContextAccessor _context;
        AuthenticationResponse user;

        public ClientController(IUserService userService, IHttpContextAccessor context, IProductServices ProductService, ITrasantionalService TrasantationService
            , IBeneficiaryServices beneficiaryServices)
        {
            this.userService = userService;
            this.ProductService = ProductService;
            this.TrasantationService = TrasantationService;
            this.beneficiaryServices = beneficiaryServices;
            _context = context;
            user = context.HttpContext.Session.Get<AuthenticationResponse>("user");
        }
        public async Task<IActionResult> SaveMoney()
        {
            var items = await ProductService.GetAllViewModelAsync();
            items = items.Where(client => client.IdClient == user.Id).ToList();

            ViewBag.CreditCards = items.Where(client => client.IdAccount == (int)TypesAccountEnum.Tarjetadecredito).ToList();

            return View("SaveMoney", new SaveMoneyVM() { HasError = false });
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
                return View("SaveMoney", vm);
            }

            var CreditCard = await ProductService.GetByIdSaveViewModelAsync(vm.IdcreditCard);

            if ((CreditCard.Amount - CreditCard.Paid) - (vm.Amount + (vm.Amount * 0.0625)) < 0)
            {
                vm.HasError = true;
                vm.Error = "No posee suficiente dinero en la tarjeta de credito para realizar dicho avance de efectivo.";
                return View("SaveMoney", vm);
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

            value = await TrasantationService.UpdateAsync(trasantion, trasantion.Id);

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

            if (SaveAccount.Code == vm.CodeSaveAccount)
            {
                vm.HasError = true;
                vm.Error = "No se puede realizar una transacion a una misma cuenta.";
                return View("Transferencia", vm);
            }


            if ((SaveAccount.Amount - vm.Amount) < 0)
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
            if (Destination == SaveAccount)
            {
                vm.HasError = true;
                vm.Error = "No puedes transferir dinero a la misma cuenta de origen.";
                return View("Transferencia", vm);
            }

            if (SaveAccount.IdAccount != (int)TypesAccountEnum.Cuentadeahorro && SaveAccount.IdAccount != (int)TypesAccountEnum.CuentaPrincipal)
            {
                vm.HasError = true;
                vm.Error = "Solo esta permito cuentas de ahorros.";
                return View("Transferencia", vm);
            }

            SaveAccount.Amount -= vm.Amount;
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

        public async Task<IActionResult> Paid()
        {
            var produts = await ProductService.GetAllWithIncludeAsync();
            produts = produts.Where(pr => pr.IdClient == user.Id).ToList();

            var Benefeciaries = await beneficiaryServices.GetAllViewModelAsync();
            Benefeciaries = Benefeciaries.Where(bn => bn.UserId == user.Id).ToList();

            var SaveAccounts = produts.Where(Account => Account.IdAccount == (int)TypesAccountEnum.CuentaPrincipal
            || (int)Account.IdAccount == (int)TypesAccountEnum.Cuentadeahorro).ToList();

            var Dubs = produts.Where(Account => Account.IdAccount == (int)TypesAccountEnum.Prestamo);

            var CreditCard = produts.Where(Account => Account.IdAccount == (int)TypesAccountEnum.Tarjetadecredito);

            ViewBag.beneficiaries = Benefeciaries;
            ViewBag.saveAccounts = SaveAccounts;
            ViewBag.dubs = Dubs;
            ViewBag.creditCard = CreditCard;

            return View();
        }

        public async Task<IActionResult> ExpressPaid(ExpressPaid vm)
        {
            vm.HasError = false;

            if (!ModelState.IsValid)
            {
                vm.HasError = true;
                vm.Error = "Por favor rellenar todos los campos, de manera que se espera.";
                return View(vm);
            }

            var list = await ProductService.GetAllWithIncludeAsync();

          

            //Money Receiver
            vm.Receiver = await ProductService.GetProductByCode(vm.IdAccountToPay);

          
            if (vm.Receiver == null)
            {
                vm.HasError = true;
                vm.Error = "Esta cuenta no esta disponible para haccer transaciones.";
                return View(vm);
            }

            if (vm.Receiver.IdClient == user.Id)
            {
                vm.HasError = true;
                vm.Error = "Para hacer una tranferencia entre cuenta utilice nuestro servicio de transferencia";
                return View(vm);
            }

            //Payer
            var PayFrom = await ProductService.GetByIdSaveViewModelAsync(vm.IdAccountPaymentMaker);

            if (PayFrom == null)
            {
                vm.HasError = true;
                vm.Error = "Debes Seleccionar la cuenta de origen donde deseas realizar el pago";
                return View(vm);
            }

            if ((PayFrom.Amount) - (vm.Amount) < 0)
            {
                vm.HasError = true;
                vm.Error = "No posee suficiente dinero en la tarjeta de credito para realizar dicha transferencia.";
                return View(vm);
            }

            var products = await ProductService.GetAllWithIncludeAsync();
            var AccountReceiverVM = products.Where(x => x.Code == vm.IdAccountToPay).SingleOrDefault();

            vm.AccountReceiver = new SaveProductVM
            {
                Id = AccountReceiverVM.Id,
                Code = AccountReceiverVM.Code,
                IdAccount = AccountReceiverVM.IdAccount,
                Amount = AccountReceiverVM.Amount,
                IdClient = AccountReceiverVM.IdClient,

            };

            vm.NameReceiver = AccountReceiverVM.client.Firstname;
            vm.LastNameReceiver = AccountReceiverVM.client.Lastname;

            vm.SaveAccount = PayFrom;


            SingletonRepository.Instance.Express = vm;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ExpressPaidPost(ExpressPaid vm)
        {

            vm = SingletonRepository.Instance.Express;

            SingletonRepository.Instance.Express = new();

            vm.SaveAccount.Amount -= vm.Amount;
            vm.SaveAccount.Paid += vm.Amount;

            bool value = await ProductService.UpdateAsync(vm.SaveAccount, vm.IdAccountPaymentMaker);

            if (!value)
            {
                vm.HasError = true;
                vm.Error = "Ha ocurrido un error intentando hacer el pago expreso .";
                return View(vm);
            }

            vm.AccountReceiver.Amount += vm.Amount;

            value = await ProductService.UpdateAsync(vm.AccountReceiver, vm.AccountReceiver.Id);
            if (!value)
            {
                vm.SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(vm.SaveAccount, vm.IdAccountPaymentMaker);

                vm.HasError = true;
                vm.Error = "Ha ocurrido un error intentando hacer el pago expreso, por favor llamar al servicio tecnico .";
                return View(vm);
            }


            var trasantion = await TrasantationService.GetByDateTrasations();
            if (trasantion == null)
            {
                vm.AccountReceiver.Amount -= vm.Amount;
                await ProductService.UpdateAsync(vm.AccountReceiver, vm.AccountReceiver.Id);

                vm.SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(vm.SaveAccount, vm.IdAccountPaymentMaker);

                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema interno, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }

            trasantion.Paids += 1;

            value = await TrasantationService.UpdateAsync(trasantion, trasantion.Id);

            if (!value)
            {
                vm.AccountReceiver.Amount -= vm.Amount;
                await ProductService.UpdateAsync(vm.AccountReceiver, vm.AccountReceiver.Id);

                vm.SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(vm.SaveAccount, vm.IdAccountPaymentMaker);


                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }


            return RedirectToRoute(new { controller = "Client", action = "Paid" });

        }

        public async Task<IActionResult> CreditCardPaid(CreditCardPaid vm)
        {
            vm.HasError = false;

            if (!ModelState.IsValid)
            {
                vm.HasError = true;
                vm.Error = "Por favor rellenar todos los campos, de manera que se espera.";
                return View(vm);
            }

            var SaveAccount = await ProductService.GetByIdSaveViewModelAsync(vm.IdSaveAccount);

            if ((SaveAccount.Amount - vm.Amount) < 0)
            {
                vm.HasError = true;
                vm.Error = "No posee suficiente balance para realizar esta pago";
                return View(vm);
            }

            var CreditCard = await ProductService.GetByIdSaveViewModelAsync(vm.IdCreditCard);

            if (CreditCard.Paid == 0)
            {
                vm.HasError = true;
                vm.Error = $"Su tarjeta de credito esta saldada";
                return View(vm);
            }

            if (CreditCard.Paid - vm.Amount < 0)
            {
                vm.HasError = true;
                vm.Error = $"La cantidad exacta a pagar es de {CreditCard.Paid}, favor intentar de nuevo con este mismo monto.";
                return View(vm);
            }

            SaveAccount.Amount -= vm.Amount;

            bool value = await ProductService.UpdateAsync(SaveAccount, vm.IdSaveAccount);

            if (!value)
            {
                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema interno cuando se estaba realizando la transferencia de cuenta a prestammo, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }

            CreditCard.Paid -= vm.Amount;

            value = await ProductService.UpdateAsync(CreditCard, vm.IdCreditCard);

            if (!value)
            {
                SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(SaveAccount, vm.IdSaveAccount);

                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema interno, cuando se estaba debitando el prestamo, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }

            var trasantion = await TrasantationService.GetByDateTrasations();

            if (trasantion == null)
            {
                CreditCard.Paid += vm.Amount;
                await ProductService.UpdateAsync(CreditCard, vm.IdCreditCard);

                SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(SaveAccount, vm.IdSaveAccount);

                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema interno, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }

            trasantion.Paids += 1;

            value = await TrasantationService.UpdateAsync(trasantion, trasantion.Id);

            if (!value)
            {
                CreditCard.Paid += vm.Amount;
                await ProductService.UpdateAsync(CreditCard, vm.IdCreditCard);

                SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(SaveAccount, vm.IdSaveAccount);

                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        public async Task<IActionResult> DuebPaid(DeubsPaids vm)
        {
            vm.HasError = false;

            if (!ModelState.IsValid)
            {
                vm.HasError = true;
                vm.Error = "Por favor rellenar todos los campos, de manera que se espera.";
                return View(vm);
            }

            var SaveAccount = await ProductService.GetByIdSaveViewModelAsync(vm.IdSaveAccount);

            if ((SaveAccount.Amount - vm.Amount) < 0)
            {
                vm.HasError = true;
                vm.Error = "No posee suficiente balance para realizar esta pago";
                return View(vm);
            }

            var deubs = await ProductService.GetByIdSaveViewModelAsync(vm.IdDeubs);

            if ((deubs.Amount - deubs.Paid) == 0)
            {
                vm.HasError = true;
                vm.Error = $"Esta prestamo fue completado, si desea otro por favor pongase en contacto con alguno de nuestros administradores.";
                return View(vm);
            }

            if ((deubs.Amount - deubs.Paid) - vm.Amount < 0)
            {
                vm.HasError = true;
                vm.Error = $"La cantidad exacta a pagar es de {deubs.Amount - deubs.Paid}, favor intentar de nuevo con este mismo monto.";
                return View(vm);
            }

            SaveAccount.Amount -= vm.Amount;

            bool value = await ProductService.UpdateAsync(SaveAccount, vm.IdSaveAccount);

            if (!value)
            {
                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema interno cuando se estaba realizando la transferencia de cuenta a prestammo, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }

            deubs.Paid += vm.Amount;

            value = await ProductService.UpdateAsync(deubs, vm.IdDeubs);

            if (!value)
            {
                SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(SaveAccount, vm.IdSaveAccount);

                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema interno, cuando se estaba debitando el prestamo, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }

            var trasantion = await TrasantationService.GetByDateTrasations();

            if (trasantion == null)
            {
                deubs.Paid -= vm.Amount;
                await ProductService.UpdateAsync(deubs, vm.IdDeubs);

                SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(SaveAccount, vm.IdSaveAccount);

                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema interno, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }

            trasantion.Paids += 1;

            value = await TrasantationService.UpdateAsync(trasantion, trasantion.Id);

            if (!value)
            {
                deubs.Paid -= vm.Amount;
                await ProductService.UpdateAsync(deubs, vm.IdDeubs);

                SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(SaveAccount, vm.IdSaveAccount);

                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        public async Task<IActionResult> BeneficiaryPaid(BeneficaryPaid vm)
        {
            vm.HasError = false;

            if (!ModelState.IsValid)
            {
                vm.HasError = true;
                vm.Error = "Por favor rellenar todos los campos, de manera que se espera.";
                return View(vm);
            }

            var SaveAccount = await ProductService.GetByIdSaveViewModelAsync(vm.IdSaveAccount);

            if ((SaveAccount.Amount - vm.Amount) < 0)
            {
                vm.HasError = true;
                vm.Error = "No posee suficiente balance para realizar esta pago";
                return View(vm);
            }

            vm.Beneficiary = await beneficiaryServices.GetByIdSaveViewModelAsync(vm.IdBeneficiary);

            if (vm.Beneficiary == null)
            {
                vm.HasError = true;
                vm.Error = $"La persona que intenta depositar no tiene acceso a este proudcto por favor comuniquese con el/ella, y pregunte por otra de nuestras cuentas.";
                return View(vm);
            }

            var products = await ProductService.GetAllWithIncludeAsync();

            var AccountBeneficiaryVM = products.Where(x => x.Code == vm.Beneficiary.BeneficiaryCode).SingleOrDefault();

            vm.AccountBeneficiary = new SaveProductVM
            {
                Id = AccountBeneficiaryVM.Id,
                Code = AccountBeneficiaryVM.Code,
                IdAccount = AccountBeneficiaryVM.IdAccount,
                Amount = AccountBeneficiaryVM.Amount,
                IdClient = AccountBeneficiaryVM.IdClient,

            };

            vm.Firstname = AccountBeneficiaryVM.client.Firstname;
            vm.Lastname = AccountBeneficiaryVM.client.Lastname;


            vm.SaveAccount = SaveAccount;

            SingletonRepository.Instance.Beneficary = vm;
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> BeneficiaryPaidPost(BeneficaryPaid vm)
        {
            vm = SingletonRepository.Instance.Beneficary;

            SingletonRepository.Instance.Beneficary = new();

            vm.SaveAccount.Amount -= vm.Amount;

            bool value = await ProductService.UpdateAsync(vm.SaveAccount, vm.IdSaveAccount);

            if (!value)
            {
                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema interno cuando se estaba realizando la transferencia de cuenta origen a cuenta destino, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }

            vm.AccountBeneficiary.Amount += vm.Amount;

            value = await ProductService.UpdateAsync(vm.AccountBeneficiary, vm.AccountBeneficiary.Id);

            if (!value)
            {
                vm.SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(vm.SaveAccount, vm.IdSaveAccount);

                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema interno, cuando se estaba debitando a la cuenta destino, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }

            var trasantion = await TrasantationService.GetByDateTrasations();

            if (trasantion == null)
            {
                vm.AccountBeneficiary.Amount -= vm.Amount;
                await ProductService.UpdateAsync(vm.AccountBeneficiary, vm.AccountBeneficiary.Id);

                vm.SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(vm.SaveAccount, vm.IdSaveAccount);

                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema interno, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }

            trasantion.Paids += 1;

            value = await TrasantationService.UpdateAsync(trasantion, trasantion.Id);

            if (!value)
            {
                vm.AccountBeneficiary.Amount -= vm.Amount;
                await ProductService.UpdateAsync(vm.AccountBeneficiary, vm.AccountBeneficiary.Id);

                vm.SaveAccount.Amount += vm.Amount;
                await ProductService.UpdateAsync(vm.SaveAccount, vm.IdSaveAccount);


                vm.HasError = true;
                vm.Error = $"Ha ocurrido un problema, favor de comunicarse con alguno de nuestros administradores.";
                return View(vm);
            }
            return RedirectToRoute(new { controller = "Home", action = "Index" });

        }
    }


}
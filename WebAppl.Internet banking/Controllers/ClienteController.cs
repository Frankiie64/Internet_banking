using Internet_banking.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAppl.Internet_banking.Controllers
{

    [Authorize(Roles = "Basic")]
    public class ClienteController : Controller
    {
        private readonly IUserService userService;

        public ClienteController(IUserService userService)
        {
           this.userService = userService;
        }

        //public Task<IActionResult> Index()
        //{
        //    return View();
        //}

        public IActionResult Beneficiarios()
        {
            return View("Beneficiarios");
        }

        public IActionResult CreateBeneficiarios()
        {
            return View("CreateBeneficiarios");
        }

        //public Task<IActionResult> RemoveBeneficiarios()
        //{
        //    return View("Beneficiarios");
        //}

        //public Task<IActionResult> Pagos()
        //{
        //    return View("Beneficiarios");
        //}

        //public Task<IActionResult> AvancesEfectivo()
        //{
        //    return View("Beneficiarios");
        //}

        //public Task<IActionResult> TransferenciaCuenta()
        //{
        //    return View("Beneficiarios");
        //}



    }
}

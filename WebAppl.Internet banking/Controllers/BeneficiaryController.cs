using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.helper;
using Internet_banking.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAppl.Internet_banking.Controllers
{

    [Authorize(Roles = "Basic")]
    public class BeneficiaryController : Controller
    {
        private readonly IUserService userService;
        private readonly IHttpContextAccessor _context;
        AuthenticationResponse user;


        public BeneficiaryController(IUserService userService, IHttpContextAccessor context)
        {
           this.userService = userService;
            _context = context;
            user = context.HttpContext.Session.Get<AuthenticationResponse>("user");
        }


    

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

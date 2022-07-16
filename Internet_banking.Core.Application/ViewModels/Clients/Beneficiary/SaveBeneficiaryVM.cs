using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Clients.Beneficiary
{
    public class SaveBeneficiaryVM
    {
        public int Id { get; set; }
        public string UserId { get; set; } // My ID

        [Required(ErrorMessage = "Debes ingresar un numero de cuenta")]
        public int BeneficiaryCode { get; set; }
    }
}

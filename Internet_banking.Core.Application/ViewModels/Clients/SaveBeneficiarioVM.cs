using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Clients
{
    public class SaveBeneficiarioVM
    {
        public string IdClient { get; set; }

        [Required(ErrorMessage = "Debes ingresar un numero de cuenta")]
        public int IdAccount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Users.Client
{
    public class SaveClienteVM: SaveUserVM
    {

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "Debes ingresar un monto apartir de 0. ")]
        public double amount { get; set; }
    }
}

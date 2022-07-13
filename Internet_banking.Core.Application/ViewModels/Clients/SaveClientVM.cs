using Internet_banking.Core.Application.ViewModels.Products;
using Internet_banking.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Clients
{
    public class SaveClientVM : SaveUserVM
    {
        [Required(ErrorMessage = "Debes ingresar el tipo de cuenta que desea agregar ")]
        [Range(1, int.MaxValue, ErrorMessage = "Este campo es obligatorio")]
        public int IdProduct { get; set; }
        [Required(ErrorMessage = "Debes ingresar un saldo apartir de 0 $RD ")]
        [Range(0, int.MaxValue, ErrorMessage = "Este campo es obligatorio")]
        public SaveProductVM FirstAccount { get; set; }
    }
}

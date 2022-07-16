using Internet_banking.Core.Application.ViewModels.TypeAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Products
{
    public class SaveProductVM
    {
        public int Id { get; set; }
        public int IdAccount { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "Debes ingresar un monto apartir de 0. ")]
        public double Amount { get; set; }
        public string IdClient { get; set; }
    }
}

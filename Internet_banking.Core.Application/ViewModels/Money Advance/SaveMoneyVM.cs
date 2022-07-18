using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Money_Advance
{
    public class SaveMoneyVM
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes ingresar una de tus tarjetas.")]
        public int IdcreditCard { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "Debes ingresar un monto apartir de 0. ")]
        public int CodeSaveAccount { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "Debes ingresar un monto apartir de 0. ")]
        public double Amount { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }

    }
}

using Internet_banking.Core.Application.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Express_Paid
{
    public class ExpressPaid
    {

        [Required(ErrorMessage = "Debes ingresar el numero desde donde quieres realizar el pago")]

        public int IdAccountPaymentMaker { get; set; }

        [Required(ErrorMessage = "Debes ingresar el numero de cuenta a quien quieres realizar el pago")]
        public int IdAccountToPay { get; set; }

        [Required(ErrorMessage = "Debes ingresar el monto a pagar")]
        [Range(0, double.MaxValue, ErrorMessage = "Debes ingresar un monto apartir de 0. ")]
        public double Amount { get; set; }
        public SaveProductVM Receiver { get; set; }

        public string NameReceiver { get; set; }
        public string LastNameReceiver { get; set; }
        public SaveProductVM SaveAccount { get; set; }
        public SaveProductVM AccountReceiver { get; set; }

        public bool HasError { get; set; }
        public string Error { get; set; }

    }
}

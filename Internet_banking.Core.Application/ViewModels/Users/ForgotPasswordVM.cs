using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Users
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Debes de colocar el correo del usuario. ")]
        [DataType(DataType.Text)]
        public string Email { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Users
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Debes de colocar el correo del usuario. ")]
        [DataType(DataType.Text)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Debes de colocar la contraseña. ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
    }
}

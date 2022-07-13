using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Users
{
    public class ResetPasswordVM
    {
        [Required(ErrorMessage = "Debes de colocar el correo del usuario. ")]
        [DataType(DataType.Text)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Debes de tener un token. ")]
        [DataType(DataType.Text)]
        public string Token { get; set; }
        [Required(ErrorMessage = "Debes Ingresar una Contraseña")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Required(ErrorMessage = "debes confirmar tu contraseña")]
        [Compare(nameof(Password), ErrorMessage = "Ambas contraseña deben coincidir")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
    }
}

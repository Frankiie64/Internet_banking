using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.ViewModels.Users
{
    public class SaveUserVM
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Debes ingresar tu Nombre")]
        [DataType(DataType.Text)]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Debes Ingresar tu Apellido")]
        [DataType(DataType.Text)]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Debes Ingresar tu Cedula")]
        [DataType(DataType.Text)]
        public string DocumementId { get; set; }

        [Required(ErrorMessage = "Debes Ingresar tu Correo")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debes Ingresar un Usuario")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Debes Ingresar el numero de telefono")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Debes Ingresar una Contraseña")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Required(ErrorMessage = "Este Campo es Obligatorio")]
        [Compare(nameof(Password), ErrorMessage = "Ambas contraseña deben coincidir")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Rol { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }

    }
}

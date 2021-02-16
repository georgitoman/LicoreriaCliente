using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Por favor, introduce un nombre de usuario")]
        [Display(Name = "Username")]
        public String UserName { get; set; }

        [Required(ErrorMessage = "Por favor, introduce tu nombre")]
        [Display(Name = "Nombre")]
        public String Nombre { get; set; }

        [Required(ErrorMessage = "Por favor, introduce una contraseña")]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 100, MinimumLength = 6, ErrorMessage = "La contraseña debe contener al menos 6 caracteres")]
        [Display(Name = "Contraseña")]
        public String Password { get; set; }

        [Required(ErrorMessage = "Por favor, confirme su contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Contraseñas no coinciden")]
        [Display(Name = "Confirmar contraseña")]
        public String ConfPassword { get; set; }

        [Required(ErrorMessage = "Por favor, introduce tu email")]
        [EmailAddress]
        [Display(Name = "Correo")]
        public String Correo { get; set; }

        [Required(ErrorMessage = "Por favor, introduce tu direccion")]
        [Display(Name = "Direccion")]
        public String Direccion { get; set; }

        [Phone(ErrorMessage = "Formato de telefono no valido")]
        [Display(Name = "Telefono")]
        public String Telefono { get; set; }
    }
}

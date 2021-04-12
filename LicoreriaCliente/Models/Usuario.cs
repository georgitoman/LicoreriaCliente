using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LicoreriaCliente.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public String UserName { get; set; }
        public String Nombre { get; set; }
        public String Password { get; set; }
        public String Salt { get; set; }
        public String Correo { get; set; }
        public String Direccion { get; set; }
        public String Telefono { get; set; }
        public int Rol { get; set; }
        public bool Validado { get; set; }
    }
}

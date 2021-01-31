using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Models
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("IDUSUARIO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdUsuario { get; set; }
        [Column("USERNAME")]
        public String UserName { get; set; }
        [Column("NOMBRE")]
        public String Nombre { get; set; }
        [Column("PASSWORD")]
        public byte[] Password { get; set; }
        [Column("SALT")]
        public String Salt { get; set; }
        [Column("VALIDADO")]
        public bool Validado { get; set; }
        [Column("CORREO")]
        public String Correo { get; set; }
        [Column("TELEFONO")]
        public String Telefono { get; set; }
        [Column("ROL")]
        public int Rol { get; set; }
    }
}

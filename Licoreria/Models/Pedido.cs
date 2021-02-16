using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Models
{
    [Table("PEDIDO")]
    public class Pedido
    {
        [Key]
        [Column("IDPEDIDO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPedido { get; set; }
        [Column("IDUSUARIO")]
        public int Usuario { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
        [Column("COSTE")]
        public decimal Coste { get; set; }
        [Column("DIRECCION")]
        public String Direccion { get; set; }
    }
}

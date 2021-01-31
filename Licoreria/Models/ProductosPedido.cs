using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Models
{
    [Table("PRODUCTOSPEDIDO")]
    public class ProductosPedido
    {
        [Key]
        [Column("IDPRODUCTOSPEDIDO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProductosPedido { get; set; }
        [Column("IDPEDIDO")]
        public int Pedido { get; set; }
        [Column("IDPRODUCTO")]
        public int Producto { get; set; }
        [Column("CANTIDAD")]
        public int Cantidad { get; set; }
    }
}

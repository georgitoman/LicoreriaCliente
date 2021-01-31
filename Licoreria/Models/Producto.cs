using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Models
{
    [Table("PRODUCTO")]
    public class Producto
    {
        [Key]
        [Column("IDPRODUCTO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProducto { get; set; }
        [Column("NOMBRE")]
        public String Nombre { get; set; }
        [Column("PRECIO")]
        public decimal Precio { get; set; }
        [Column("STOCK")]
        public int Stock { get; set; }
        [Column("IMAGEN")]
        public String Imagen { get; set; }
        [Column("LITROS")]
        public decimal Litros { get; set; }
        [Column("IDCATEGORIA")]
        public int Categoria { get; set; }
    }
}

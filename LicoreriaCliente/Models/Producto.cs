using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LicoreriaCliente.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public String Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public String Imagen { get; set; }
        public decimal Litros { get; set; }
        public int Categoria { get; set; }
    }
}

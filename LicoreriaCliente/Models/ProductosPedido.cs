using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LicoreriaCliente.Models
{
    public class ProductosPedido
    {
        public int IdProductosPedido { get; set; }
        public int Pedido { get; set; }
        public int Producto { get; set; }
        public int Cantidad { get; set; }
    }
}

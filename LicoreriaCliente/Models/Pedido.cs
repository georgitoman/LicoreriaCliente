using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LicoreriaCliente.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Coste { get; set; }
        public String Direccion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicoreriaCliente.Models
{
    public class Carrito
    {
        public List<int> Productos { get; set; }
        public List<int> Cantidades { get; set; }
    }
}

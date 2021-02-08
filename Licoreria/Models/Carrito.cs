using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Models
{
    public class Carrito
    {
        public List<int> Productos { get; set; }
        public List<int> Cantidades { get; set; }
    }
}

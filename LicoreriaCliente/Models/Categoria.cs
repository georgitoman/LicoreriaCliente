using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LicoreriaCliente.Models
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public String Nombre { get; set; }
    }
}

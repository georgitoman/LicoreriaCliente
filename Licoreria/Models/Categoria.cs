using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Models
{
    [Table("CATEGORIA")]
    public class Categoria
    {
        [Key]
        [Column("IDCATEGORIA")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdCategoria { get; set; }

        [Column("NOMBRE")]
        public String Nombre { get; set; }
    }
}

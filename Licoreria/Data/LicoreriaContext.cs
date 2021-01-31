using Licoreria.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Data
{
    public class LicoreriaContext: DbContext
    {
        public LicoreriaContext(DbContextOptions options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ProductosPedido> ProductosPedidos { get; set; }
    }
}

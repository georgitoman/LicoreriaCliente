using Licoreria.Data;
using Licoreria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Repositories
{
    public class RepositoryLicoreria
    {
        LicoreriaContext context;

        public RepositoryLicoreria(LicoreriaContext context)
        {
            this.context = context;
        }

        public List<Categoria> GetCategorias()
        {
            return this.context.Categorias.Where(z => z.IdCategoria != 6).ToList();
        }

        public List<Producto> GetProductos(int idcategoria)
        {
            return this.context.Productos.Where(z => z.Categoria == idcategoria).ToList();
        }

        public List<Producto> GetProductosMini()
        {
            return this.context.Productos.Where(z => z.Litros <= 0.02M).ToList();
        }

        public List<Producto> GetProductosMaxi()
        {
            return this.context.Productos.Where(z => z.Litros >= 1.50M).ToList();
        }
    }
}

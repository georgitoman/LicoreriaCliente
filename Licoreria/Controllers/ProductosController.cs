using Licoreria.Models;
using Licoreria.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Controllers
{
    public class ProductosController : Controller
    {
        RepositoryLicoreria repo;

        public ProductosController(RepositoryLicoreria repo)
        {
            this.repo = repo;
        }

        public IActionResult Index(int idcategoria)
        {
            List<Producto> productos = repo.GetProductos(idcategoria);
            return View(productos);
        }

        public IActionResult MicroAlcoholismo()
        {
            List<Producto> productos = repo.GetProductosMini();
            return View(productos);
        }

        public IActionResult MacroAlcoholismo()
        {
            List<Producto> productos = repo.GetProductosMaxi();
            return View(productos);
        }
    }
}

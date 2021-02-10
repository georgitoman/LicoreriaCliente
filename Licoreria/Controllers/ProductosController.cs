using Licoreria.Extensions;
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
        IRepositoryLicoreria repo;

        public ProductosController(IRepositoryLicoreria repo)
        {
            this.repo = repo;
        }

        public IActionResult Index(int idcategoria)
        {
            List<Producto> productos = repo.GetProductos(idcategoria);
            ViewData["NOMBRECAT"] = this.repo.GetNombreCategoria(idcategoria);
            TempData["CATEGORIA"] = idcategoria;
            return View(productos);
        }

        [HttpPost]
        public IActionResult Index(int idproducto, int cantidad)
        {
            Carrito sessioncar;
            if (HttpContext.Session.GetObject<Carrito>("CARRITO") == null)
            {
                sessioncar = new Carrito();
                sessioncar.Productos = new List<int>();
                sessioncar.Cantidades = new List<int>();
            }
            else
            {
                sessioncar = HttpContext.Session.GetObject<Carrito>("CARRITO");
            }
            if (sessioncar.Productos.Contains(idproducto) == false)
            {
                sessioncar.Productos.Add(idproducto);
                sessioncar.Cantidades.Add(cantidad);
                HttpContext.Session.SetObject("CARRITO", sessioncar);
            } else
            {
                int i = sessioncar.Productos.IndexOf(idproducto);
                sessioncar.Cantidades[i] += cantidad;
                HttpContext.Session.SetObject("CARRITO", sessioncar);
            }

            int idcategoria = Convert.ToInt32(TempData["CATEGORIA"]);
            ViewData["NOMBRECAT"] = this.repo.GetNombreCategoria(idcategoria);
            TempData["CATEGORIA"] = idcategoria;
            List<Producto> productos = this.repo.GetProductos(idcategoria);

            return View(productos);
        }

        public IActionResult MicroAlcoholismo()
        {
            List<Producto> productos = repo.GetProductosMini();
            return View(productos);
        }

        [HttpPost]
        public IActionResult MicroAlcoholismo(int idproducto, int cantidad)
        {
            Carrito sessioncar;
            if (HttpContext.Session.GetObject<Carrito>("CARRITO") == null)
            {
                sessioncar = new Carrito();
                sessioncar.Productos = new List<int>();
                sessioncar.Cantidades = new List<int>();
            }
            else
            {
                sessioncar = HttpContext.Session.GetObject<Carrito>("CARRITO");
            }
            if (sessioncar.Productos.Contains(idproducto) == false)
            {
                sessioncar.Productos.Add(idproducto);
                sessioncar.Cantidades.Add(cantidad);
                HttpContext.Session.SetObject("CARRITO", sessioncar);
            }
            else
            {
                int i = sessioncar.Productos.IndexOf(idproducto);
                sessioncar.Cantidades[i] += cantidad;
                HttpContext.Session.SetObject("CARRITO", sessioncar);
            }
            List<Producto> productos = repo.GetProductosMini();
            return View(productos);
        }

        public IActionResult MacroAlcoholismo()
        {
            List<Producto> productos = repo.GetProductosMaxi();
            return View(productos);
        }

        [HttpPost]
        public IActionResult MacroAlcoholismo(int idproducto, int cantidad)
        {
            Carrito sessioncar;
            if (HttpContext.Session.GetObject<Carrito>("CARRITO") == null)
            {
                sessioncar = new Carrito();
                sessioncar.Productos = new List<int>();
                sessioncar.Cantidades = new List<int>();
            }
            else
            {
                sessioncar = HttpContext.Session.GetObject<Carrito>("CARRITO");
            }
            if (sessioncar.Productos.Contains(idproducto) == false)
            {
                sessioncar.Productos.Add(idproducto);
                sessioncar.Cantidades.Add(cantidad);
                HttpContext.Session.SetObject("CARRITO", sessioncar);
            }
            else
            {
                int i = sessioncar.Productos.IndexOf(idproducto);
                sessioncar.Cantidades[i] += cantidad;
                HttpContext.Session.SetObject("CARRITO", sessioncar);
            }
            List<Producto> productos = repo.GetProductosMaxi();
            return View(productos);
        }

        public IActionResult Carrito(int? pos, int? cantidad)
        {
            Carrito sessioncar = HttpContext.Session.GetObject<Carrito>("CARRITO");
            if(sessioncar == null)
            {
                ViewData["MENSAJE"] = "CARRITO VACÍO";
                return View();
            }
            else if (sessioncar.Productos.Count == 0)
            {
                ViewData["MENSAJE"] = "CARRITO VACÍO";
                return View();
            }
            else
            {
                List<Producto> productos = this.repo.GetListaProductos(sessioncar.Productos);
                ViewData["CANTIDADES"] = sessioncar.Cantidades;
                return View(productos);
            }
        }

        [HttpPost]
        public IActionResult Carrito(int pos, int cantidad)
        {
            Carrito sessioncar = HttpContext.Session.GetObject<Carrito>("CARRITO");
            if (cantidad == 0)
            {
                sessioncar.Productos.RemoveAt(pos);
                sessioncar.Cantidades.RemoveAt(pos);
            }
            else
            {
                sessioncar.Cantidades[pos] = cantidad;
            }
            HttpContext.Session.SetObject("CARRITO", sessioncar);

            return RedirectToAction("Carrito", "Productos");
        }
    }
}

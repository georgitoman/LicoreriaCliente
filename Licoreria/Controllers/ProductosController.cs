using Licoreria.Extensions;
using Licoreria.Helpers;
using Licoreria.Models;
using Licoreria.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Controllers
{
    public class ProductosController : Controller
    {
        IRepositoryLicoreria repo;
        UploadService UploadService;

        public ProductosController(IRepositoryLicoreria repo, UploadService us)
        {
            this.repo = repo;
            this.UploadService = us;
        }

        public IActionResult TodosProductos(int? posicion, String nombre,
            decimal? preciomax, decimal? litros, bool? stock,
            int? idcategoria)
        {
            if(posicion == null)
            {
                posicion = 1;
            }
            int ultimo = 0;

            List<Producto> productos = repo.GetProductos(posicion.Value, ref ultimo, nombre,
                preciomax, litros, stock, idcategoria);

            if (nombre != null)
            {
                ViewData["NOMBRE"] = nombre;
                ViewData["BUSQUEDA"] = "Resultados de busqueda: " + nombre;
            }
            if (preciomax != null)
            {
                ViewData["PRECIOMAX"] = preciomax.Value;
            }
            if (litros != null)
            {
                ViewData["LITROS"] = litros.Value;
            }
            if (stock != null)
            {
                ViewData["STOCK"] = stock.Value;
            }
            if (idcategoria != null)
            {
                ViewData["IDCATEGORIA"] = idcategoria.Value;
                ViewData["TITULO"] = this.repo.GetNombreCategoria(idcategoria.Value);
                ViewData["MAX"] = this.repo.GetPrecioMax(idcategoria.Value);
                ViewData["MIN"] = this.repo.GetPrecioMin(idcategoria.Value);
            }
            else
            {
                ViewData["MAX"] = this.repo.GetPrecioMax(null);
                ViewData["MIN"] = this.repo.GetPrecioMin(null);
            }

            ViewData["ULTIMO"] = ultimo;
            
            ViewData["LISTALITROS"] = this.repo.GetListaLitros();

            return View(productos);
        }

        [HttpPost]
        public IActionResult TodosProductos(String nombre,
            decimal? preciomax, decimal? litros, String stock,
            int? idcategoria)
        {
            if (nombre != null)
            {
                ViewData["NOMBRE"] = nombre;
                ViewData["BUSQUEDA"] = "Resultados de busqueda: " + nombre;
            }
            if (litros != null)
            {
                ViewData["LITROS"] = litros.Value;
            }
            bool? boolstock = null;
            if (stock != null)
            {
                if (stock == "true")
                {
                    ViewData["STOCK"] = true;
                    boolstock = true;
                }
                else
                    ViewData["STOCK"] = null;
            }
            if (idcategoria != null)
            {
                ViewData["IDCATEGORIA"] = idcategoria.Value;
                ViewData["TITULO"] = this.repo.GetNombreCategoria(idcategoria.Value);
                ViewData["MAX"] = this.repo.GetPrecioMax(idcategoria.Value);
                ViewData["MIN"] = this.repo.GetPrecioMin(idcategoria.Value);
            } else
            {
                ViewData["MAX"] = this.repo.GetPrecioMax(null);
                ViewData["MIN"] = this.repo.GetPrecioMin(null);
            }
            if (preciomax != null)
            {
                if (preciomax == (decimal)ViewData["MAX"])
                    ViewData["PRECIOMAX"] = null;
                else
                    ViewData["PRECIOMAX"] = preciomax.Value;
            }

            ViewData["LISTALITROS"] = this.repo.GetListaLitros();
            int ultimo = 0;
            
            List<Producto> productos = repo.GetProductos(1, ref ultimo, nombre,
                preciomax, litros, boolstock, idcategoria);

            ViewData["ULTIMO"] = ultimo;

            return RedirectToAction("TodosProductos", "Productos", new
            {
                posicion = ViewData["POSICION"],
                nombre = ViewData["NOMBRE"],
                preciomax = ViewData["PRECIOMAX"],
                litros = ViewData["LITROS"],
                stock = ViewData["STOCK"],
                idcategoria = ViewData["IDCATEGORIA"]
            });
        }

        [HttpPost]
        public IActionResult AddCarrito(int idproducto, int cantidad, String redirect)
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

            int stock = this.repo.GetStock(idproducto);
            if (sessioncar.Productos.Contains(idproducto) == false)
            {
                if (stock < cantidad)
                {
                    cantidad = stock;
                    TempData["ALERTA"] = "No hay suficiente stock. Se han metido " + cantidad + " productos a su carrito.";
                }
                sessioncar.Productos.Add(idproducto);
                sessioncar.Cantidades.Add(cantidad);
                HttpContext.Session.SetObject("CARRITO", sessioncar);
            }
            else
            {
                int i = sessioncar.Productos.IndexOf(idproducto);
                if (stock < cantidad)
                {
                    if (sessioncar.Cantidades[i] == stock)
                        TempData["ALERTA"] = "No hay suficiente stock.";
                    else
                        TempData["ALERTA"] = "No hay suficiente stock. Se han metido " + (stock - sessioncar.Cantidades[i]) + " productos extras a su carrito.";
                    cantidad = stock;
                }
                sessioncar.Cantidades[i] = cantidad;
                HttpContext.Session.SetObject("CARRITO", sessioncar);
            }

            return Redirect(redirect);
        }

        public IActionResult Carrito()
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

        #region ADMIN

        public IActionResult GestionIndex()
        {
            return View();
        }

        public IActionResult InsertarProducto()
        {
            ViewData["CATEGORIAS"] = this.repo.GetCategorias();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertarProducto(String nombre, decimal precio, int stock, IFormFile imagen, decimal litros, int idcategoria)
        {
            String filename = null;
            if(imagen != null)
            {
                FileInfo fi = new FileInfo(imagen.FileName);
                String extension = fi.Extension;
                filename = ToolkitService.NormalizeName(extension, nombre, litros.ToString());
                await this.UploadService.UploadFileAsync(imagen, Folders.Images, filename);
            }

            this.repo.InsertarProducto(nombre, precio, stock, filename, litros, idcategoria);
            return RedirectToAction("GestionIndex", "Productos");
        }

        public IActionResult EditarProducto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditarProducto(String nombre, decimal precio, int stock, String imagen, decimal litros, int idcategoria)
        {
            return View();
        }

        #endregion
        
    }
}

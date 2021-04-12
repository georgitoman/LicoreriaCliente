using LicoreriaCliente.Extensions;
using LicoreriaCliente.Filters;
using LicoreriaCliente.Helpers;
using LicoreriaCliente.Models;
using LicoreriaCliente.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LicoreriaCliente.Controllers
{
    public class ProductosController : Controller
    {
        LicoreriaService repo;
        ImagesService ImagesService;

        public ProductosController(LicoreriaService repo, ImagesService imagesService)
        {
            this.repo = repo;
            this.ImagesService = imagesService;
        }

        public async Task<IActionResult> TodosProductos(String nombre,
            decimal? preciomax, decimal? litros, bool? stock,
            int? idcategoria)
        {
            List<Producto> productos = await repo.GetProductosAsync(nombre,
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
                ViewData["TITULO"] = await this.repo.GetNombreCategoriaAsync(idcategoria.Value);
            }

            ViewData["MAX"] = await this.repo.GetPrecioMaxAsync(idcategoria);
            ViewData["MIN"] = await this.repo.GetPrecioMinAsync(idcategoria);
            ViewData["LISTALITROS"] = await this.repo.GetListaLitrosAsync();

            return View(productos);
        }

        [HttpPost]
        public async Task<IActionResult> TodosProductos(String nombre,
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
                ViewData["TITULO"] = await this.repo.GetNombreCategoriaAsync(idcategoria.Value);
            }

            ViewData["MAX"] = await this.repo.GetPrecioMaxAsync(idcategoria);
            ViewData["MIN"] = await this.repo.GetPrecioMinAsync(idcategoria);
            ViewData["LISTALITROS"] = await this.repo.GetListaLitrosAsync();

            if (preciomax != null)
            {
                if (preciomax == (decimal)ViewData["MAX"])
                    ViewData["PRECIOMAX"] = null;
                else
                    ViewData["PRECIOMAX"] = preciomax.Value;
            }
            
            List<Producto> productos = await repo.GetProductosAsync(nombre,
                preciomax, litros, boolstock, idcategoria);


            return RedirectToAction("TodosProductos", "Productos", new
            {
                nombre = ViewData["NOMBRE"],
                preciomax = ViewData["PRECIOMAX"],
                litros = ViewData["LITROS"],
                stock = ViewData["STOCK"],
                idcategoria = ViewData["IDCATEGORIA"]
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddCarrito(int idproducto, int cantidad, String redirect)
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

            int stock = await this.repo.GetStockAsync(idproducto);
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

        public async Task<IActionResult> Carrito()
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
                List<Producto> productos = await this.repo.GetListaProductosAsync(sessioncar.Productos);
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

        [AuthorizeAdmin]
        public IActionResult GestionIndex()
        {
            return View();
        }

        [AuthorizeAdmin]
        public async Task<IActionResult> InsertarProducto()
        {
            ViewData["CATEGORIAS"] = await this.repo.GetCategoriasAsync();
            return View();
        }

        [AuthorizeAdmin]
        [HttpPost]
        public async Task<IActionResult> InsertarProducto(String nombre, decimal precio, int stock, IFormFile imagen, decimal litros, int idcategoria)
        {
            String filename = null;
            if(imagen != null)
            {
                FileInfo fi = new FileInfo(imagen.FileName);
                String extension = fi.Extension;
                filename = ToolkitService.NormalizeName(extension, nombre, litros.ToString());
                using(var stream = imagen.OpenReadStream())
                {
                    await this.ImagesService.UploadImageAsync(filename, stream);
                }
            }
            String token = HttpContext.Session.GetString("TOKEN");
            await this.repo.InsertarProductoAsync(nombre, precio, stock, filename, litros, idcategoria, token);
            return RedirectToAction("GestionIndex", "Productos");
        }

        [AuthorizeAdmin]
        public async Task<IActionResult> SeleccionarProducto()
        {
            List<Producto> productos = await this.repo.GetProductosAsync(null, null, null, null, null);
            return View(productos);
        }

        [AuthorizeAdmin]
        public async Task<IActionResult> EditarProducto(int idproducto)
        {
            ViewData["CATEGORIAS"] = await this.repo.GetCategoriasAsync();
            Producto producto = await this.repo.BuscarProductoAsync(idproducto);
            TempData["NOMBREIMAGEN"] = producto.Imagen;
            return PartialView("_EditarProductoPartial", producto);
        }

        [AuthorizeAdmin]
        [HttpPost]
        public async Task<IActionResult> EditarProducto(int idproducto, String nombre, decimal precio, int stock, IFormFile imagen, decimal litros, int idcategoria)
        {
            String filename = null;
            if (imagen != null)
            {
                if (TempData["NOMBREIMAGEN"] != null)
                {
                    String imagenanterior = TempData["NOMBREIMAGEN"].ToString();
                    await this.ImagesService.DeleteImageAsync(imagenanterior);
                }
                FileInfo fi = new FileInfo(imagen.FileName);
                String extension = fi.Extension;
                filename = ToolkitService.NormalizeName(extension, nombre, litros.ToString());
                using (var stream = imagen.OpenReadStream())
                {
                    await this.ImagesService.UploadImageAsync(filename, stream);
                }
            }

            String token = HttpContext.Session.GetString("TOKEN");
            await this.repo.EditarProductoAsync(idproducto, nombre, precio, stock, filename, litros, idcategoria, token);
            return RedirectToAction("SeleccionarProducto", "Productos");
        }

        [AuthorizeAdmin]
        public IActionResult EliminarProducto()
        {
            return View();
        }

        [AuthorizeAdmin]
        [HttpPost]
        public async Task<IActionResult> EliminarProducto(int idproducto)
        {
            Producto prod = await this.repo.BuscarProductoAsync(idproducto);
            String imagen = prod.Imagen;
            if(imagen != null)
            {
                await this.ImagesService.DeleteImageAsync(imagen);
            }

            String token = HttpContext.Session.GetString("TOKEN");
            await this.repo.EliminarProductoAsync(idproducto, token);
            return RedirectToAction("SeleccionarProducto", "Productos");
        }

        #endregion
        
    }
}

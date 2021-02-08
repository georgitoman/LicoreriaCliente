using Licoreria.Extensions;
using Licoreria.Models;
using Licoreria.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Controllers
{
    public class PedidosController : Controller
    {
        RepositoryLicoreria repo;

        public PedidosController(RepositoryLicoreria repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TramitarPedido()
        {
            if(this.HttpContext.Session.GetString("USER") == null)
            {
                return RedirectToAction("Login", "Usuarios");
            } else
            {
                Carrito sessioncar = HttpContext.Session.GetObject<Carrito>("CARRITO");
                List<Producto> productos = this.repo.GetListaProductos(sessioncar.Productos);
                ViewData["CANTIDADES"] = sessioncar.Cantidades;
                ViewData["SUBTOTAL"] = TempData["SUMATOTAL"];
                return View(productos);
            }
        }

        public IActionResult ConfirmarPedido(decimal subtotal)
        {
            int usuario = Convert.ToInt32(this.HttpContext.Session.GetString("USER"));
            Carrito carrito = HttpContext.Session.GetObject<Carrito>("CARRITO");
            this.repo.CreatePedido(usuario, subtotal, carrito);
            return RedirectToAction("Pedidos", "Usuarios");
        }
    }
}

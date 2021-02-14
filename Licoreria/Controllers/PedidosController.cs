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
        IRepositoryLicoreria repo;

        public PedidosController(IRepositoryLicoreria repo)
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
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("PedidosUsuario", "Pedidos");
        }

        public IActionResult PedidosUsuario()
        {
            int usuario = Convert.ToInt32(HttpContext.Session.GetString("USER"));
            List<Pedido> pedidos = this.repo.GetPedidosUsuario(usuario);
            return View(pedidos);
        }

        public IActionResult ProductosPedido(int idpedido)
        {
            List<int> cantidades = new List<int>();
            List<Producto> productos = this.repo.GetProductosPedido(idpedido, ref cantidades);
            ViewData["CANTIDADES"] = cantidades;
            return View(productos);
        }

        public IActionResult Cancelar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cancelar(int idpedido)
        {
            this.repo.CancelarPedido(idpedido);
            return RedirectToAction("PedidosUsuario", "Pedidos");
        }
    }
}

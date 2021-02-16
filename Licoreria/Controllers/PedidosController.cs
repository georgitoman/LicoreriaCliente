using Licoreria.Extensions;
using Licoreria.Filters;
using Licoreria.Models;
using Licoreria.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Licoreria.Controllers
{
    [AuthorizeUsuario]
    public class PedidosController : Controller
    {
        IRepositoryLicoreria repo;

        public PedidosController(IRepositoryLicoreria repo)
        {
            this.repo = repo;
        }

        public IActionResult TramitarPedido()
        {
            Carrito sessioncar = HttpContext.Session.GetObject<Carrito>("CARRITO");
            List<Producto> productos = this.repo.GetListaProductos(sessioncar.Productos);
            ViewData["CANTIDADES"] = sessioncar.Cantidades;
            ViewData["SUBTOTAL"] = TempData["SUMATOTAL"];
            return View(productos);
        }

        public IActionResult ConfirmarPedido(decimal subtotal)
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idusuario = int.Parse(dato);
            Carrito carrito = HttpContext.Session.GetObject<Carrito>("CARRITO");
            this.repo.CreatePedido(idusuario, subtotal, carrito);
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("PedidosUsuario", "Pedidos");
        }

        public IActionResult PedidosUsuario()
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idusuario = int.Parse(dato);
            List<Pedido> pedidos = this.repo.GetPedidosUsuario(idusuario);
            return View(pedidos);
        }

        public IActionResult ProductosPedido(int idpedido)
        {
            Pedido ped = this.repo.BuscarPedido(idpedido);
            List<int> cantidades = new List<int>();
            List<Producto> productos = this.repo.GetProductosPedido(idpedido, ref cantidades);
            ViewData["CANTIDADES"] = cantidades;
            ViewData["DIRECCION"] = ped.Direccion;
            ViewData["SUBTOTAL"] = ped.Coste;
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

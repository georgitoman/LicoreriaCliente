using LicoreriaCliente.Extensions;
using LicoreriaCliente.Filters;
using LicoreriaCliente.Models;
using LicoreriaCliente.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LicoreriaCliente.Controllers
{
    [AuthorizeUsuario]
    public class PedidosController : Controller
    {
        LicoreriaService repo;

        public PedidosController(LicoreriaService repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> TramitarPedido()
        {
            Carrito sessioncar = HttpContext.Session.GetObject<Carrito>("CARRITO");
            List<Producto> productos = await this.repo.GetListaProductosAsync(sessioncar.Productos);
            ViewData["CANTIDADES"] = sessioncar.Cantidades;
            ViewData["SUBTOTAL"] = TempData["SUMATOTAL"];
            return View(productos);
        }

        public async Task<IActionResult> ConfirmarPedido(decimal subtotal)
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idusuario = int.Parse(dato);
            Carrito carrito = HttpContext.Session.GetObject<Carrito>("CARRITO");
            String token = HttpContext.Session.GetString("TOKEN");
            await this.repo.CreatePedidoAsync(idusuario, subtotal, carrito, token);
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("PedidosUsuario", "Pedidos");
        }

        public async Task<IActionResult> PedidosUsuario()
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idusuario = int.Parse(dato);
            String token = HttpContext.Session.GetString("TOKEN");
            List<Pedido> pedidos = await this.repo.GetPedidosUsuarioAsync(idusuario, token);
            return View(pedidos);
        }

        public async Task<IActionResult> ProductosPedido(int idpedido)
        {
            String token = HttpContext.Session.GetString("TOKEN");
            Pedido ped = await this.repo.BuscarPedidoAsync(idpedido, token);
            Carrito pp = await this.repo.GetProductosPedidoAsync(idpedido, token);
            List<Producto> productos = await this.repo.GetListaProductosAsync(pp.Productos);
            ViewData["CANTIDADES"] = pp.Cantidades;
            ViewData["DIRECCION"] = ped.Direccion;
            ViewData["SUBTOTAL"] = ped.Coste;
            return View(productos);
        }

        public IActionResult Cancelar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cancelar(int idpedido)
        {
            String token = HttpContext.Session.GetString("TOKEN");
            await this.repo.CancelarPedidoAsync(idpedido, token);
            return RedirectToAction("PedidosUsuario", "Pedidos");
        }
    }
}

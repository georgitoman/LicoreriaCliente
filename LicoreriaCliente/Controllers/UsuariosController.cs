using LicoreriaCliente.Filters;
using LicoreriaCliente.Models;
using LicoreriaCliente.Services;
using LicoreriaCliente.ViewModels;
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
    public class UsuariosController : Controller
    {
        LicoreriaService repo;

        public UsuariosController(LicoreriaService repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Perfil()
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idusuario = int.Parse(dato);
            String token = HttpContext.Session.GetString("TOKEN");
            Usuario user = await this.repo.BuscarUsuarioAsync(idusuario, token);
            return View(user);
        }

        public async Task<IActionResult> EditarUsuario()
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idusuario = int.Parse(dato);
            String token = HttpContext.Session.GetString("TOKEN");
            Usuario user = await this.repo.BuscarUsuarioAsync(idusuario, token);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(String nombre, String direccion, String telefono)
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idusuario = int.Parse(dato);
            String token = HttpContext.Session.GetString("TOKEN");
            await this.repo.EditarUsuarioAsync(idusuario, nombre, direccion, telefono, token);

            return RedirectToAction("Perfil", "Usuarios");
        }

        public IActionResult CambiarContraseña()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CambiarContraseña(String cont, String newcont, String repnewcont)
        {
            String username = User.FindFirst(ClaimTypes.Name).Value;
            String token = await this.repo.GetToken(username, cont);
            Usuario user = await this.repo.GetUsuarioAsync(token);

            if (user == null)
            {
                ViewData["CONT"] = "Contraseña incorrecta";
            } else
            {
                if(newcont.Length < 6)
                {
                    ViewData["NEWCONT"] = "La contraseña debe tener al menos 6 caracteres";
                } else
                {
                    if(newcont != repnewcont)
                    {
                        ViewData["REPNEWCONT"] = "Las contraseñas deben coinidir";
                    } else
                    {
                        await this.repo.CambiarContraseñaAsync(user.IdUsuario, newcont, token);
                        return RedirectToAction("EditarUsuario", "Usuarios");
                    }
                }
            }

            return View();
        }

    }
}

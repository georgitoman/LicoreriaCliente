using Licoreria.Filters;
using Licoreria.Models;
using Licoreria.Repositories;
using Licoreria.ViewModels;
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
    public class UsuariosController : Controller
    {
        IRepositoryLicoreria repo;

        public UsuariosController(IRepositoryLicoreria repo)
        {
            this.repo = repo;
        }

        public IActionResult Perfil()
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idusuario = int.Parse(dato);
            Usuario user = this.repo.BuscarUsuario(idusuario);
            return View(user);
        }

        public IActionResult EditarUsuario()
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idusuario = int.Parse(dato);
            Usuario user = this.repo.BuscarUsuario(idusuario);
            return View(user);
        }

        [HttpPost]
        public IActionResult EditarUsuario(String nombre, String direccion, String telefono)
        {
            String dato = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idusuario = int.Parse(dato);
            this.repo.EditarUsuario(idusuario, nombre, direccion, telefono);

            return RedirectToAction("Perfil", "Usuarios");
        }

        public IActionResult CambiarContraseña()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CambiarContraseña(String cont, String newcont, String repnewcont)
        {
            String username = User.FindFirst(ClaimTypes.Name).Value;

            Usuario user = this.repo.LoginUsuario(username, cont);

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
                        this.repo.CambiarContraseña(user.IdUsuario, newcont);
                        return RedirectToAction("EditarUsuario", "Usuarios");
                    }
                }
            }

            return View();
        }

    }
}

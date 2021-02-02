using Licoreria.Models;
using Licoreria.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Controllers
{
    public class UsuariosController : Controller
    {
        RepositoryLicoreria repo;

        public UsuariosController(RepositoryLicoreria repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(String username, String password)
        {
            Usuario user = this.repo.LoginUsuario(username, password);

            if(user != null)
            {
                return RedirectToAction("Index", "Home");
            } else
            {
                ViewData["MENSAJE"] = "<p style='color:red'>Usuario o contraseña incorrectos</p>";
                return View();
            }
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(String username, String nombre, String correo, String password, String confpassword, String telefono)
        {
            if(password == confpassword)
            {
                this.repo.InsertarUsuario(username, nombre, correo, password, telefono);
                return RedirectToAction("LogIn", "Usuarios");
            }
            else
            {
                ViewData["MENSAJE"] = "<h4 style='color:red'>Contraseñas no coinciden</h4>";
                return View();
            }
            
        }
    }
}

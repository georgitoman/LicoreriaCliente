using Licoreria.Models;
using Licoreria.Repositories;
using Licoreria.ViewModels;
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
        public IActionResult Registro(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (this.repo.UserNameExists(model.UserName))
                {
                    ViewData["MENSAJE"] = "Nombre de usuario ya esta en uso";
                    return View();
                } else
                {
                    this.repo.InsertarUsuario(model.UserName, model.Nombre, model.Correo, model.Password, model.Telefono);
                    return RedirectToAction("LogIn", "Usuarios");
                }

            } else
            {
                return View();
            }

        }
    }
}

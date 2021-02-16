using Licoreria.Models;
using Licoreria.Repositories;
using Licoreria.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Licoreria.Controllers
{
    public class IdentityController : Controller
    {
        IRepositoryLicoreria repo;

        public IdentityController(IRepositoryLicoreria repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(String username, String password)
        {
            Usuario user = this.repo.LoginUsuario(username, password);

            if (user != null)
            {
                ClaimsIdentity identidad = new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);
                identidad.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()));
                identidad.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                identidad.AddClaim(new Claim(ClaimTypes.Role, user.Rol.ToString()));
                ClaimsPrincipal principal = new ClaimsPrincipal(identidad);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.Now.AddMinutes(15)
                    });

                String action = TempData["action"].ToString();
                String controller = TempData["controller"].ToString();
                return RedirectToAction(action, controller);
            }
            else
            {
                ViewData["MENSAJE"] = "<p style='color:red'>Usuario o contraseña incorrectos</p>";
                return View();
            }
        }

        public IActionResult AccesoDenegado()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("TodosProductos", "Productos");
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
                }
                else
                {
                    this.repo.InsertarUsuario(model.UserName, model.Nombre, model.Correo, model.Password, model.Direccion, model.Telefono);
                    return RedirectToAction("Perfil", "Usuarios");
                }
            }
            else
            {
                return View();
            }
        }
    }
}

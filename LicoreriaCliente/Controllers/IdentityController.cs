using LicoreriaCliente.Models;
using LicoreriaCliente.Services;
using LicoreriaCliente.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LicoreriaCliente.Controllers
{
    public class IdentityController : Controller
    {
        LicoreriaService repo;

        public IdentityController(LicoreriaService repo)
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
            String token = await this.repo.GetToken(username, password);

            if (token == null)
            {
                ViewData["MENSAJE"] = "<p style='color:red'>Usuario o contraseña incorrectos</p>";
                return View();
            } else
            {
                Usuario user = await this.repo.GetUsuarioAsync(token);
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
                        ExpiresUtc = DateTime.Now.AddMinutes(60)
                    });
                HttpContext.Session.SetString("TOKEN", token);

                String action = TempData["action"].ToString();
                String controller = TempData["controller"].ToString();
                return RedirectToAction(action, controller);
            }
        }

        public IActionResult AccesoDenegado()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (HttpContext.Session.GetString("TOKEN") != null)
            {
                HttpContext.Session.Remove("TOKEN");
            }
            return RedirectToAction("TodosProductos", "Productos");
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await this.repo.UserNameExistsAsync(model.UserName))
                {
                    ViewData["MENSAJE"] = "Nombre de usuario ya esta en uso";
                    return View();
                }
                else
                {
                    await this.repo.InsertarUsuarioAsync(model.UserName, model.Nombre, model.Correo, model.Password, model.Direccion, model.Telefono);
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

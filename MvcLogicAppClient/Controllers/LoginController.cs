using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcLogicAppClient.Services;
using SeguridadApiAlumnosPractica.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcLogicAppClient.Controllers
{
    public class LoginController : Controller
    {

        private ServiceCliente service;

        public LoginController(ServiceCliente service)
        {
            this.service = service;
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string nombre, string apellidos)
        {
            string token =
                await this.service.GetTokenAsync(nombre, apellidos);
            if (token == null)
            {
                ViewData["MENSAJE"] = "Nombre/Apellido incorrectos";
                return View();
            }
            else
            {
                //SI EL USUARIO EXISTE, ALMACENAMOS EL TOKEN EN SESSION
                ClaimsIdentity identity =
                    new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim("TOKEN", token));

                identity.AddClaim(new Claim("nombre", nombre));
                identity.AddClaim(new Claim("apellidos", apellidos));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , principal, new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });
                return RedirectToAction("Alumnos", "Home");
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}

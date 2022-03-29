using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcLogicAppClient.Models;
using MvcLogicAppClient.Services;
using SeguridadApiAlumnosPractica.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MvcLogicAppClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        ServiceCliente service;

        public HomeController(ILogger<HomeController> logger, ServiceCliente service)
        {
            this.service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Alumnos()
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            List<Alumno> alumnos = await this.service.GetAlumnosAsync(token);

            return View(alumnos);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            Alumno alumno = await this.service.GetAlumnoAsync(id,token);
            return View(alumno);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Alumno alumno)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            await this.service.CreateAlumno(alumno.IdAlumno, alumno.Curso, alumno.Nombre, alumno.Apellidos, alumno.Nota, token);
            return RedirectToAction("Alumnos");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            await this.service.DeleteAlumno(id, token);
            return RedirectToAction("Alumnos");
        }

        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            Alumno alumno = await this.service.GetAlumnoAsync(id,token);
            return View(alumno);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(Alumno alumno)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            await this.service.UpdateAlumno(alumno.IdAlumno,alumno.Curso,alumno.Nombre,alumno.Apellidos,alumno.Nota,token);
            return RedirectToAction("Alumnos");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

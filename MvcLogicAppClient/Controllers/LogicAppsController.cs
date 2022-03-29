using Microsoft.AspNetCore.Mvc;
using MvcLogicAppClient.Services;
using SeguridadApiAlumnosPractica.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcLogicAppClient.Controllers
{
    public class LogicAppsController : Controller
    {
        private ServiceCliente service;

        public LogicAppsController(ServiceCliente service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Alumnos()
        {
            List<Alumno> alumnos = await this.service.GetAlumnosFlowAsync();
            return View(alumnos);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Logging;
using Application.Services;
using Presentation.Web.Context;
using DTO = Application.DTO;
using Enums = Domain.Infrastructure;

using Infrastructure.Utils.Extensions;
using System.Globalization;

namespace Presentation.Web.Controllers
{
    /// <summary>
    /// Controller que devuelve datos Json
    /// </summary>
    [SiteContext]
    public class JsonController : Controller
    {

        private readonly Commons active = new Commons();

        /// <summary>
        /// Fecha de Fin de Plazo
        /// </summary>
        /// <param name="FechaIngreso"></param>
        /// <param name="PlazoDias"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult FechaFinPlazo(string FechaIngreso, int PlazoDias)
        {
            DateTime fecha = Convert.ToDateTime(FechaIngreso).Date;
            int cont = 0;
            int diasPlazo = PlazoDias; 

            IList<DTO.Models.Feriado> feriados = active.GetFeriados();

            while (cont < diasPlazo)
            {
                fecha = fecha.AddDays(1);

                if ((fecha.DayOfWeek != DayOfWeek.Sunday) && !feriados.Any(x => x.Fecha == fecha))
                {
                    cont++;
                }
            }

            return Json(new { result = fecha.ToString("dd/MM/yyyy") });
        }
    }
}
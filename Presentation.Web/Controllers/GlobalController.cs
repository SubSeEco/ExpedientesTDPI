using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Domain.Infrastructure;
using Infrastructure.Logging;

namespace Presentation.Web.Controllers
{
    /// <summary>
    /// Controller base
    /// </summary>
    public class GlobalController : Controller
    {
        internal const string TipoUploadChar = "{#}";

        /// <summary>
        /// Devuelve un response 403
        /// </summary>
        /// <returns></returns>
        public JsonResult Response403()
        {
            HttpContext.Response.Status = "403 Forbidden";
            HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            HttpContext.Response.End();
            return null;
        }
               
    }
}
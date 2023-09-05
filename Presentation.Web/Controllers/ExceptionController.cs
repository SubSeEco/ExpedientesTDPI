using Presentation.Web.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Web.Controllers
{
    /// <summary>
    /// Controlados de manejo de errores.
    /// </summary>
    [SiteContext]
    public class ExceptionController : Controller
    {
        /// <summary>
        /// Vista principal de error
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region Set Error
        /// <summary>
        /// Despliegue de Error 400
        /// </summary>
        /// <returns></returns>
        public ActionResult Error400()
        {
            throw new HttpException((int)System.Net.HttpStatusCode.BadRequest, "400 BadRequest");
        }

        /// <summary>
        /// Despliegue de Error 403
        /// </summary>
        /// <returns></returns>
        public ActionResult Error403()
        {
            throw new HttpException((int)System.Net.HttpStatusCode.Forbidden, "403 Forbidden");
        }

        /// <summary>
        /// Despliegue de Error 404
        /// </summary>
        /// <returns></returns>
        public ActionResult Error404()
        {
            throw new HttpException((int)System.Net.HttpStatusCode.NotFound, "404 NotFound");
        }

        /// <summary>
        /// Despliegue de Error 500
        /// </summary>
        /// <returns></returns>
        public ActionResult Error500()
        {
            throw new HttpException((int)System.Net.HttpStatusCode.InternalServerError, "500 InternalServerError");
        }

        #endregion
    }
}
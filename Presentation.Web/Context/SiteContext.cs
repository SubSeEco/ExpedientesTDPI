using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.Threading;
using System.Globalization;
using Domain.Infrastructure;


namespace Presentation.Web.Context
{
    /// <summary>
    /// Contexto del sistema, se usa en todos los Controllers
    /// </summary>
    public class SiteContext : ActionFilterAttribute
    {
        /// <summary>
        /// Envía variables de contexto ViewBag a todas las vistas
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LanguageEnum language;
            string cultureCode;
            CultureInfo culture;

            //filterContext.Controller.ViewBag.isCoordinador = false;
            //filterContext.Controller.ViewBag.isNivelCentral = false;

            if (!String.IsNullOrEmpty((string)filterContext.RouteData.Values["language"]))
            {
                cultureCode = (string)filterContext.RouteData.Values["language"];
                language = (LanguageEnum)Enum.Parse(typeof(LanguageEnum), cultureCode, true);

                if (language == LanguageEnum.es || language == LanguageEnum.ES)
                {
                    cultureCode = LanguageEnum.es.ToString() + "-CL";
                    filterContext.Controller.ViewBag.Language = LanguageEnum.ES.ToString();
                }
                else
                {
                    cultureCode = LanguageEnum.en.ToString() + "-US";
                    filterContext.Controller.ViewBag.Language = LanguageEnum.EN.ToString();
                }

                culture = new CultureInfo(cultureCode);
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            }
            else
            {

                if (filterContext.HttpContext.Request.Cookies["LanguageCk"] != null)
                {
                    string cultureCodeCookie = filterContext.HttpContext.Request.Cookies["LanguageCk"].Value;
                    language = (LanguageEnum)Enum.Parse(typeof(LanguageEnum), cultureCodeCookie, true);

                    if (language == LanguageEnum.es || language == LanguageEnum.ES)
                    {
                        cultureCode = LanguageEnum.es.ToString() + "-CL";
                        filterContext.Controller.ViewBag.Language = LanguageEnum.ES.ToString();
                    }
                    else
                    {
                        cultureCode = LanguageEnum.en.ToString() + "-US";
                        filterContext.Controller.ViewBag.Language = LanguageEnum.EN.ToString();
                    }

                    culture = new CultureInfo(cultureCode);
                    System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                }
                else
                {
                    cultureCode = LanguageEnum.es.ToString() + "-CL";
                    filterContext.Controller.ViewBag.Language = LanguageEnum.ES.ToString();
                    culture = new CultureInfo(cultureCode);
                    System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                }
            }
        }
    }
}
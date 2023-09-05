using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Presentation.Web
{
    /// <summary>
    /// RouteConfig
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// RegisterRoutes
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "Default_1",
                url: "{language}/{controller}/{action}/{id}/{id2}",
                defaults: new
                {
                    language = "ES",
                    controller = "Main",
                    //controller = "Account",
                    action = "Index",
                    id = UrlParameter.Optional,
                    id2 = UrlParameter.Optional
                },
                namespaces: new string[] { "Presentation.Web.Controllers" }
            );

            routes.MapRoute(
                name: "EscritorioCausas",
                url: "{language}/Expedientes",
                defaults: new { controller = "Expedientes", action = "Index" }
            );

            routes.MapRoute(
                name: "ActionInitialSystem",
                url: "{language}/Main",
                defaults: new { controller = "Main", action = "Index" }
            );

            routes.MapRoute(
                name: "VistaPublicaAcceso",
                url: "{language}/Account",
                defaults: new { controller = "Account", action = "Index" }
            );

            routes.MapRoute(
                name: "VistaPublicaDashboard",
                url: "{language}/Main/Dashboard",
                defaults: new { controller = "Main", action = "Dashboard" }
            );

        }



    }
}

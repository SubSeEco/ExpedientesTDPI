using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Presentation.Web
{
    /// <summary>
    /// WebApiConfig
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// WebApiConfig => Register
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            //GlobalConfiguration.Configure(x => x.MapHttpAttributeRoutes());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.Formatters.XmlFormatter.UseXmlSerializer = true;
            //var xml = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            //xml.UseXmlSerializer = true;
        }
    }
}

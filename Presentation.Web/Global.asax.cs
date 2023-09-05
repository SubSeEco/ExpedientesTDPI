using Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Presentation.Web
{
    /// <summary>
    /// MvcApplication
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application_Start
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.EnsureInitialized();

            //GlobalConfiguration.Configuration.Formatters.XmlFormatter.UseXmlSerializer = true;

            Logger.Configure("SGDE.WEBAPP");
            Logger.Execute().Info("BeginLogger");
            ApiFeriado.Feriados();
        }
    }
}

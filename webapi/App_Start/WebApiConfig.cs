using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;
using webapi.Controllers;
using webapi.Core;

namespace webapi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Serviços e configuração da API da Web
                
            // Rotas da API da Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.Indent = true;

            Thread T1 = new Thread(APIs.RefreshTemp);
            T1.Start();
        }
    }
}

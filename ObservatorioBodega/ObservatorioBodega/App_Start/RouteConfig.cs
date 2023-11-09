using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ObservatorioBodega
{
    public class RouteConfig
    {

        //para probar funcion debido a fallo del enrutador de abajo
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
        //        public static void RegisterRoutes(RouteCollection routes)
        //        {
        //            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


        //            routes.MapRoute(
        //               name: "Menus",
        //               url: "{controller}/{action}/{id}",
        //               defaults: new { controller = "Menus", action = "Index", id = UrlParameter.Optional }
        //            );

        //            routes.MapRoute(
        //               name: "Documentos",
        //               url: "{controller}/{action}/{id}",
        //               defaults: new { controller = "Documentos", action = "Index", id = UrlParameter.Optional }
        //            );

        //            //RUTA PARA COLABORADORES
        //            routes.MapRoute(
        //            name: "Colaboradores",
        //            url: "{controller}/{action}/{id}",
        //            defaults: new { controller = "Colaboradores", action = "Index", id = UrlParameter.Optional }
        //);
        //        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CulturaSurvey
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

     

            routes.MapRoute(
               name: "camu",
               url: "reg/{id}",
               defaults: new { controller = "Creg", action = "CamuRegistration", id = UrlParameter.Optional }
           );
            routes.MapRoute(
             name: "creg",
             url: "Creg/reg/{id}",
             defaults: new { controller = "Creg", action = "Registration_Edit", id = UrlParameter.Optional }
         );


            routes.MapRoute(
               name: "Event",
               url: "{id}",
               defaults: new { controller = "Event", action = "Job", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "creg_report",
               url: "creg_report/{id}",
               defaults: new { controller = "Creg", action = "creg_report", id = UrlParameter.Optional }
           );
            routes.MapRoute(
            name: "Sales",
            url: "sales/{id}",
            defaults: new { controller = "Meterial", action = "Sales", id = UrlParameter.Optional }
        );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }
            );

        }
    }
}

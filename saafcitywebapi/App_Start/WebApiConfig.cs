using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace saafcitywebapi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Custom route for GetImage action
            config.Routes.MapHttpRoute(
                name: "GetImage",
                routeTemplate: "api/Complaint/image/{Complaint_ID}",
                defaults: new { controller = "Complaint", action = "GetImage", Complaint_ID = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

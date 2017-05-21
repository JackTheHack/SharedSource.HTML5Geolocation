using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace SharedSource.HTML5Geolocation.Pipelines
{
    public class InitializeRoutes
    {
        public void Process(PipelineArgs args)
        {
            var routes = RouteTable.Routes;
            routes.MapRoute("AnalyticsUpdate", "HTML5GeoApi/UpdateGeolocation",
             new
             {
                 controller = "HTML5GeoApi",
                 action = "UpdateGeolocation",
                 id = UrlParameter.Optional
             });

        }
    }
}
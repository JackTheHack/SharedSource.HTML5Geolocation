using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharedSource.LogTail.Helpers;
using Sitecore.Analytics;

namespace SharedSource.LogTail.Controller
{
    public class HTML5GeoApiController : System.Web.Mvc.Controller
    {
        public ActionResult UpdateGeolocation()
        {
            if (Tracker.Current == null || Tracker.Current.Interaction == null)
            {
                return HttpNotFound("Tracker is not initialized");
            }

            if (GeoIpHelper.UpdateGeoIpFromCookie(Tracker.Current.Interaction))
            {

                return Content("OK");
            }
            else
            {
                return HttpNotFound("Error during update");
            }
        }
    }
}
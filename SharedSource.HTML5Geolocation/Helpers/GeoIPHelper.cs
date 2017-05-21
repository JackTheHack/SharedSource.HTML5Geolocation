using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
using Sitecore.Analytics.Model;
using Sitecore.Analytics.Tracking;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace SharedSource.LogTail.Helpers
{
    public class GeoIpHelper
    {
        public static bool UpdateGeoIpFromCookie(CurrentInteraction interaction)
        {
            bool html5Geolocation = Settings.GetBoolSetting("Analytics.HTML5Geolocation.Enabled", true);

            bool html5GeolocationSuccess = false;

            if (html5Geolocation && HttpContext.Current.Request.Cookies.AllKeys.Contains("Analytics.HTML5ReverseLookupResult"))
            {
                Log.Info("Analytics - Trying to resolve address by HTML5 geolocation cookie.", typeof(GeoIpHelper));

                var geolocationCookie = HttpContext.Current.Request.Cookies["Analytics.HTML5ReverseLookupResult"];

                if (geolocationCookie != null && !string.IsNullOrEmpty(geolocationCookie.Value))
                {
                    var cookieValue = WebUtility.UrlDecode(geolocationCookie.Value);

                    try
                    {
                        dynamic geoData = JObject.Parse(cookieValue);

                        var whois = new WhoIsInformation
                        {
                            Latitude = geoData.Lat,
                            Longitude = geoData.Long,
                            PostalCode = geoData.PostalCode,
                            Country = geoData.Country,
                            City = geoData.City,
                            Region = geoData.Region
                        };

                        interaction.SetGeoData(whois);

                        html5GeolocationSuccess = true;
                    }
                    catch (Exception e)
                    {
                        Log.Error("Analytics - Error during HTML5 geolocation", e, typeof(GeoIpHelper));
                    }
                }
            }

            return html5GeolocationSuccess;
        }
    }
}
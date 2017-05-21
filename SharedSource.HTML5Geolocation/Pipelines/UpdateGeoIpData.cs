using System;
using SharedSource.LogTail.Helpers;
using Sitecore.Analytics.Pipelines.CreateVisits;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace SharedSource.HTML5Geolocation.Pipelines
{
    public class UpdateGeoIpData : CreateVisitProcessor
    {
        public override void Process(CreateVisitArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            var intSetting = Settings.GetIntSetting("HTML5.Geolocation.Timeout", 120);

            var html5GeolocationSuccess = GeoIpHelper.UpdateGeoIpFromCookie(args.Interaction); ;

            if (!html5GeolocationSuccess)
            {
                //We can fallback to other methods of geolocation if it failed.
                Log.Info("Analytics - HTML5 geolocation is not allowed by user. Falling back to IP lookup.", this);
                args.Interaction.UpdateGeoIpData(TimeSpan.FromSeconds(intSetting));
            }
        }
    }
}
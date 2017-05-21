# Sitecore.HTML5Geolocation
This NuGet package enables to support the HTML5 geolocation in Sitecore analytics

# Installation

Run the following command in NuGet Manager Console:

```Install-Package SharedSource.HTML5Geolocation -Version 1.0.0.0 -Source https://www.myget.org/F/jackspektor/api/v3/index.json```

# Dependencies and requirements
- Sitecore 7.5 and higher
- jQuery installed and referenced
- Google Maps API script installed and referenced
- jQuery Cookies installed and referenced
- HTTPS binding for the website (otherwise HTML5 geolocation will not be enabled in Chrome)

# Usage Example

```<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBvvvzizg87c8T-gRzJ_YNuBj7J7T-UWh8"></script>
<script type="text/javascript" src="/Scripts/jquery-1.6.4.min.js"></script>
<script type="text/javascript" src="/Scripts/js-cookie.js"></script>
<script type="text/javascript" src="/Scripts/GeolocationModule.js"></script>
<script type="text/javascript">
	var geolocationModule = GeoModule(jQuery, Cookies);
	geolocationModule.initializeGeolocation();
</script>

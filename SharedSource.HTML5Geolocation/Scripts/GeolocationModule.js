var GeoModule = function ($, Cookies) {
    // private

    //Some helper functions here...
    var findMatch = function (results, test) {
        for (var i = 0; i < results.length; i++) {
            if (test(results[i])) {
                return results[i];
            }
        }

        return undefined;
    }

    var findResult = function (results, name) {
        var result = findMatch(results, function (obj) {
            return obj.types[0] === name;
        });
        return result ? result.long_name : null;
    };

    var findResultShortName = function (results, name) {
        var result = findMatch(results, function (obj) {
            return obj.types[0] === name;
        });
        return result ? result.short_name : null;
    };

    //This function will save the geolocation data to the cookie and perform the reverse geocoding using Google Maps API
    //I use the jquery.cookies plugin to simplify the cookies access
    function showPosition(position) {
        console.log("Geolocation - Latitude: " + position.coords.latitude + " Longitude: " + position.coords.longitude);
        var expiryDate = new Date();
        expiryDate.setMilliseconds(expiryDate.getMilliseconds() + 2147483647);
        Cookies.remove('HTML5Geolocation');
        Cookies.set('HTML5Geolocation', position.coords.latitude + "," + position.coords.longitude, { expires: expiryDate });

        var geocoder = new google.maps.Geocoder;
        var latlng = { lat: position.coords.latitude, lng: position.coords.longitude };

        var geolocationCookie = Cookies.get('Analytics.HTML5ReverseLookupResult');

        //We don't want to perform reverse geocode each request - only if there was no cookie with cached location
        if (!geolocationCookie) {
            geocoder.geocode({ 'location': latlng }, function (results, status) {
                if (status === google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        console.log("Found geocode result.");
                        var addressResults = results[0].address_components;
                        var reverseLookupResult = {
                            Lat: position.coords.latitude,
                            Long: position.coords.longitude,
                            PostalCode: findResult(addressResults, 'postal_code'),
                            City: findResult(addressResults, 'locality'),
                            Country: findResultShortName(addressResults, 'country'),
                            Region: findResult(addressResults, 'administrative_area_level_1')
                        };

                        console.log(reverseLookupResult)

                        //We got the results - lets save it
                        Cookies.remove('Analytics.HTML5ReverseLookupResult');
                        Cookies.set('Analytics.HTML5ReverseLookupResult', JSON.stringify(reverseLookupResult), { expires: expiryDate })

                        //And we also want to update the Sitecore geolocation data with our location immidiately by calling the webservice
                        $.ajax({ url: "/HTML5GeoApi/UpdateGeolocation" });

                    } else {
                        console.log('No geocode results found');
                    }
                } else {
                    console.log('Geocoder failed due to: ' + status);
                }
            });
        } else {
            //So we already completed geolocation before...
            console.log("Geocode was already completed - using the cache location.");
            console.log(Cookies.get('Analytics.HTML5ReverseLookupResult'));
        }
    }

    // public
    return {
        initializeGeolocation: function () {
            $(document).ready(function () {
                //Most modern browsers support HTML5 geolocation, but lets check first!
                if (navigator.geolocation) {
                    console.log("Requesing geolocation from browser...");

                    navigator.geolocation.getCurrentPosition(showPosition, function () {
                        //Unfortunately user can deny to give you access to their location.
                        console.log("Geolocation request was denied. Falling back to IP lookup for personalisation purposes.")
                    });
                } else {
                    console.log("Geolocation is disabled in this browser. Falling back to IP lookup for personalisation purposes.")
                }
            });
        }
    };
};
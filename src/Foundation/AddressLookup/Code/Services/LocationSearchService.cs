// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocationSearchService.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the LocationSearchService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Aceik.Foundation.AddressLookup.GeolocationSearch;
using Aceik.Foundation.AddressLookup.Models;
using Aceik.Foundation.AddressLookup.Settings;
using Sitecore.Foundation.DependencyInjection;

namespace Aceik.Foundation.AddressLookup.Services
{
    [Service(typeof(ILocationSearchService))]
    public class LocationSearchService : ILocationSearchService
    {
        private readonly IGoogleSettings googleSettings;


        public LocationSearchService(IGoogleSettings googleSettings)
        {
            this.googleSettings = googleSettings;
        }

        /// <summary>
        /// The get coordinate.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// The <see cref="LatLng"/>.
        /// </returns>
        public LatLng GetCoordinate(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return null;

            // Scope the location to the required site country.
            location = ScopeLocationToRequiredCountry(location, this.googleSettings.CountryPostFix);

            var googleGeocoder = new GoogleGeocodingProvider(this.googleSettings);
            var geocodeResults = googleGeocoder.Geocode(location);
            if (geocodeResults?.Locations != null && geocodeResults.Locations.Count != 0)
            {
                var coordinate = geocodeResults.Locations.First().Point;
                return coordinate;
            }

            return null;
        }

        /// <summary>
        /// Limits search to a specific country
        /// </summary>
        /// <param name="location">search string</param>
        /// <param name="countryScopePostFix">country code</param>
        /// <returns>Modified search string with location included</returns>
        private static string ScopeLocationToRequiredCountry(string location, string countryScopePostFix)
        {
            var coordinateRegex = new Regex(@"^(\-?\d+(\.\d+)?),\s*(\-?\d+(\.\d+)?)$", RegexOptions.None);
            if (coordinateRegex.IsMatch(location))
            {
                // Don't alter location searches that are GPS coordinates.
                return location;
            }

            // Replace instances of multiple spaces with a single space
            var regex = new Regex(@"[ ]{2,}", RegexOptions.None);
            location = regex.Replace(location, @" ");

            // Get the country postfix - default to web settings, fallback to current culture.
            string countryPostfix;
            if (!string.IsNullOrWhiteSpace(countryScopePostFix))
            {
                countryPostfix = countryScopePostFix;
            }
            else
            {
                var region = new RegionInfo(CultureInfo.CurrentCulture.Name);
                countryPostfix = ", " + region.NativeName;
            }

            // Append the country postfix if necessary
            if (!location.EndsWith(countryPostfix, StringComparison.OrdinalIgnoreCase))
            {
                location += countryPostfix;
            }

            return location;
        }
    }
}
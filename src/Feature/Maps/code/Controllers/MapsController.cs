using System.Collections.Generic;
using System.Linq;
using Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching.Services;
using Geocoding;
using Geocoding.Google;

namespace Sitecore.Feature.Maps.Controllers
{
    using System;
    using System.Web.Mvc;
    using Aceik.Foundation.CloudSpatialSearch.Models;

    public class MapsController : Mvc.Controllers.SitecoreController
    {
        private readonly ISpatialSearchService _searchOfficeService;

        public MapsController(ISpatialSearchService searhOfficeService)
        {
            _searchOfficeService = searhOfficeService;   
        }                                                          

        [HttpPost]
        [Foundation.SitecoreExtensions.Attributes.SkipAnalyticsTracking]
        public JsonResult GetMapPoints(double lat, double longitude, double radius, int maxResults = 50)
        {
            var beginSearchTime = DateTime.Now;
            var spatialResults = this._searchOfficeService.GetSpatialResultsByDistance(new Location(lat, longitude), radius, maxResults);
            var endSearchTime = DateTime.Now;
            TimeSpan spanDifference = endSearchTime - beginSearchTime;
            int msSeachTook = (int)spanDifference.TotalMilliseconds;
            MapSearchResults results = new MapSearchResults(spatialResults.ToList(), msSeachTook);
            return this.Json(results);
        }

        [HttpPost]
        [Foundation.SitecoreExtensions.Attributes.SkipAnalyticsTracking]
        public JsonResult GetAddressLocation(string searchAddress)
        {
            IGeocoder geocoder = new GoogleGeocoder() {  };
            IEnumerable<Address> addresses = geocoder.Geocode(searchAddress);

            var enumerable = addresses as IList<Address> ?? addresses.ToList();
            if (enumerable.Any())
            {
                return this.Json(new {success = true, data = enumerable.First().Coordinates});   
            }
            else
            {
                return this.Json(new { success = false });
            }
        }
    }
}
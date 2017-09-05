using System.Linq;
using Aceik.Foundation.AddressLookup.Models;
using Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching.Services;
using Sitecore.Feature.Maps.Models;

namespace Sitecore.Feature.Maps.Controllers
{
    using System;
    using System.Web.Mvc;
    using Data;
    using Aceik.Foundation.AddressLookup.Services;
    using Aceik.Foundation.CloudSpatialSearch.Models;

    public class MapsController : Mvc.Controllers.SitecoreController
    {
        private readonly ISpatialSearchService _searhOfficeService;

        public MapsController(ILocationSearchService googleSearchService, ISpatialSearchService searhOfficeService)
        {
            _searhOfficeService = searhOfficeService;
            GoogleSearchService = googleSearchService;
        }

        public ILocationSearchService GoogleSearchService { get; }

        [HttpPost]
        [Foundation.SitecoreExtensions.Attributes.SkipAnalyticsTracking]
        public JsonResult GetMapPoints(double lat, double longitude, double radius, int maxResults = 50)
        {
            var beginSearchTime = DateTime.Now;
            var spatialResults = this._searhOfficeService.GetSpatialResultsByDistance(new LatLng(lat, longitude), radius, maxResults);
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
            return this.Json(this.GoogleSearchService.GetCoordinate(searchAddress));
        }
    }
}
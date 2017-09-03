// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpatialSearchService.cs" company="FLG">
//   FLG 2017
// </copyright>
// <summary>
//   Defines the SpatialSearchService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Aceik.Foundation.AddressLookup.Models;
using Aceik.Foundation.CloudSpatialSearch.IndexRead.Core;
using Aceik.Foundation.CloudSpatialSearch.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Azure.Utils;
using Sitecore.Foundation.DependencyInjection;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching.Services
{
    [Service(typeof(ISpatialSearchService))]
    public class SpatialSearchToLinqService : ISpatialSearchService
    {
        private readonly IIndexConfiguration indexingConfiguration;
        private readonly IDistanceCalculatorService _distanceCalculator;

        public SpatialSearchToLinqService(IIndexConfiguration indexingConfiguration, IDistanceCalculatorService distanceCalculator)
        {
            this.indexingConfiguration = indexingConfiguration;
            _distanceCalculator = distanceCalculator;
        }

        public List<SpatialSearchResultItem> GetSpatialResultsAndDistance(LatLng coordinate, double searchRadius, int maxResults = 50)
        {
            IEnumerable<SpatialSearchResultItem> results = GetComputedByCoordinate<SpatialSearchResultItem>(coordinate, searchRadius, maxResults);

            List<SpatialSearchResultItem> distanceResults = new List<SpatialSearchResultItem>();
            foreach (var spatialResult in results)
            {
                var geoJson = new GeoJson { Type = "Point", Coordinates = new List<double>() };

                double latD;
                double longitudeD;
                if (!double.TryParse(spatialResult.Longitude, out longitudeD) || !double.TryParse(spatialResult.Latitude, out latD))
                {
                    // todo: log error
                    return null;
                }

                geoJson.Coordinates.Add(longitudeD);
                geoJson.Coordinates.Add(latD);

                spatialResult.Distance = this._distanceCalculator.CalculateDistanceGps(coordinate, new LatLng(geoJson.Coordinates[1], geoJson.Coordinates[0]));
                spatialResult.GeoLocation = geoJson;
                distanceResults.Add(spatialResult);
            }
            return distanceResults;
        }

        public IEnumerable<T> GetComputedByCoordinate<T>(LatLng coordinate, double settingsSearchRadius, int maxResults = 50) where T : SpatialSearchResultItem
        {
            var indexName = string.Format(this.indexingConfiguration.IndexName, Context.Database.Name.ToLower());
            using (var context = ContentSearchManager.GetIndex(indexName).CreateSearchContext())
            {
                string fieldName = Sitecore.Configuration.Settings.GetSetting("Foundation.GeoSearch.Azure.GeoJsonFieldName", "geo_location");
                var query = context.GetExtendedQueryable<SpatialSearchResultItem>().Where(item => item.GeoLocation.WithinRadius(coordinate.Latitude, coordinate.Longitude, fieldName, settingsSearchRadius, true, maxResults));
                return (IEnumerable<T>) query.AsEnumerable();
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpatialSearchToLinqService.cs" company="FLG">
//   FLG 2017
// </copyright>
// <summary>
//   Defines the SpatialSearchToLinqService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;                            
using Aceik.Foundation.CloudSpatialSearch.IndexRead.Core;
using Aceik.Foundation.CloudSpatialSearch.Models;
using Geocoding;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Azure.Utils;
using Sitecore.Foundation.DependencyInjection;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching.Services
{
    //[Service(typeof(ISpatialSearchService))]
    public class SpatialSearchToLinqService : ISpatialSearchService
    {
        private readonly IIndexConfiguration _indexingConfiguration;
        private readonly IDistanceCalculatorService _distanceCalculator;

        public SpatialSearchToLinqService(IIndexConfiguration indexingConfiguration, IDistanceCalculatorService distanceCalculator)
        {
            this._indexingConfiguration = indexingConfiguration;
            _distanceCalculator = distanceCalculator;
        }

        /// <summary>
        /// Given a coordinate, and a search radius lookup a number of results
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="searchRadius"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public List<SpatialSearchResultItem> GetSpatialResultsByDistance(Location coordinate, double searchRadius, int maxResults = 50)
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
                    continue;
                }

                geoJson.Coordinates.Add(longitudeD);
                geoJson.Coordinates.Add(latD);

                spatialResult.Distance = this._distanceCalculator.CalculateDistanceGps(coordinate, new Location(geoJson.Coordinates[1], geoJson.Coordinates[0]));
                spatialResult.GeoLocation = geoJson;
                distanceResults.Add(spatialResult);
            }
            return distanceResults.OrderBy(x => x.Distance).ToList();
        }

        /// <summary>
        /// Perform a Geo Spatial search using the ContentSearchManager and the   WithinRadius lookup.
        /// 
        /// Current limitation is that ordering and maxResults are being filtered by standard Sitecore code.  Yet to work out a way around this.
        /// If you need ordering within the query and more results, please use the direct query implementation SpatialSearchService.cs for now.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coordinate"></param>
        /// <param name="settingsSearchRadius"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public IEnumerable<T> GetComputedByCoordinate<T>(Location coordinate, double settingsSearchRadius, int maxResults = 50) where T : SpatialSearchResultItem
        {
            var indexName = string.Format(this._indexingConfiguration.IndexName, Context.Database.Name.ToLower());
            using (var context = ContentSearchManager.GetIndex(indexName).CreateSearchContext())
            {
                string fieldName = Sitecore.Configuration.Settings.GetSetting("Foundation.GeoSearch.Azure.GeoJsonFieldName", "geo_location");
                var query = context.GetExtendedQueryable<SpatialSearchResultItem>().Where(item => item.GeoLocation.WithinRadius(coordinate.Latitude, coordinate.Longitude, fieldName, settingsSearchRadius, true, maxResults));
                return (IEnumerable<T>) query.AsEnumerable();
            }
        }
    }
}
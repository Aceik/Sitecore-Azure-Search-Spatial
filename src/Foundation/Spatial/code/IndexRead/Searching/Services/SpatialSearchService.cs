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
using Aceik.Foundation.CloudSpatialSearch.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Sitecore;
using Sitecore.ContentSearch.Azure.Utils;
using Sitecore.Foundation.DependencyInjection;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching.Services
{
    [Service(typeof(ISpatialSearchService))]
    public class SpatialSearchService : ISpatialSearchService
    {
        private readonly IIndexConfiguration indexingConfiguration;
        private readonly IDistanceCalculatorService _distanceCalculator;

        public SpatialSearchService(IIndexConfiguration indexingConfiguration, IDistanceCalculatorService distanceCalculator)
        {
            this.indexingConfiguration = indexingConfiguration;
            _distanceCalculator = distanceCalculator;
        }

        /// <summary>
        /// Given a coordinate, and a search radius search for results closest to a particular coordinate.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="searchRadius"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public List<SpatialSearchResultItem> GetSpatialResultsByDistance(LatLng coordinate, double searchRadius, int maxResults = 50)
        {
            IEnumerable<SpatialSearchResultItem> results = GetComputedByCoordinate<SpatialSearchResultItem>(coordinate, searchRadius, maxResults);

            List<SpatialSearchResultItem> distanceResults = new List<SpatialSearchResultItem>();
            foreach (var spatialResult in results)
            {
                spatialResult.Distance = this._distanceCalculator.CalculateDistanceGps(coordinate, new LatLng(spatialResult.GeoLocation.Coordinates[1], spatialResult.GeoLocation.Coordinates[0]));
                distanceResults.Add(spatialResult);
            }
            return distanceResults;
        }

        /// <summary>
        /// Perform a Geo Spatial search using a direct query to the Azure Search Index. MaxResults and orderby work with this direct query.
        ///                                                                             
        /// The other way of doing this is using the other implementation ISpatialSearchService, which demonstrates using the ContentSearchManager and a linq provider.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coordinate"></param>
        /// <param name="settingsSearchRadius"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public IEnumerable<T> GetComputedByCoordinate<T>(LatLng coordinate, double settingsSearchRadius, int maxResults = 50) where T : SpatialSearchResultItem
        {
            var settingsDictionary = ConnectionStringParser.Parse(this.indexingConfiguration.AzureSearchConnectionString);

            var azureSearchServiceUrl = settingsDictionary["serviceUrl"];
            var apiKey = settingsDictionary["apiKey"];

            var azureServiceUri = new Uri(azureSearchServiceUrl, UriKind.Absolute);

            using (ISearchServiceClient searchClient = new SearchServiceClient(azureServiceUri, new SearchCredentials(apiKey)))
            {
                var indexName = string.Format(this.indexingConfiguration.IndexName.Replace("_", "-"), Context.Database.Name.ToLower());
                var index = searchClient.Indexes.GetClient(indexName);

                string fieldName = Sitecore.Configuration.Settings.GetSetting("Foundation.GeoSearch.Azure.GeoJsonFieldName", "geo_location");
                var parameters = new SearchParameters
                {
                    Filter = $"geo.distance({fieldName}, geography'POINT({coordinate.Longitude} {coordinate.Latitude})') le {settingsSearchRadius}",
                    Top =  maxResults,
                    OrderBy = new[] {$"geo.distance({fieldName}, geography'POINT({coordinate.Longitude} {coordinate.Latitude})') asc"}
                };

                var results = index.Documents.Search<T>("*", parameters).Results;
                return results.Select(d => d.Document);
            }
        }


    }
}
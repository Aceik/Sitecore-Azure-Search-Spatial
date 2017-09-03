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

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching.Services
{
    //[Service(typeof(ISpatialSearchService))]
    public class SpatialSearchService : ISpatialSearchService
    {
        private readonly IIndexConfiguration indexingConfiguration;
        private readonly IDistanceCalculatorService _distanceCalculator;

        public SpatialSearchService(IIndexConfiguration indexingConfiguration, IDistanceCalculatorService distanceCalculator)
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
                spatialResult.Distance = this._distanceCalculator.CalculateDistanceGps(coordinate, new LatLng(spatialResult.GeoLocation.Coordinates[1], spatialResult.GeoLocation.Coordinates[0]));
                distanceResults.Add(spatialResult);
            }
            return distanceResults;
        }

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
                    Filter = string.Format(
                        "geo.distance({0}, geography'POINT({1} {2})') le {3}",
                        fieldName,
                        coordinate.Longitude,
                        coordinate.Latitude,
                        settingsSearchRadius),
                    Top =  maxResults
                };

                var results = index.Documents.Search<T>("*", parameters).Results;
                return results.Select(d => d.Document);
            }
        }


    }
}
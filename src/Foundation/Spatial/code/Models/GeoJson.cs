using System.Collections.Generic;
using Newtonsoft.Json;

namespace Aceik.Foundation.CloudSpatialSearch.Models
{
    public class GeoJson
    {
        [JsonProperty(PropertyName = "coordinates")]
        public List<double> Coordinates { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Search results within radius of a specific point. This method is an entry point for Linq.
        /// </summary>
        /// <param name="latitude">Latitude of the search point</param>
        /// <param name="longitude">Logitude of the search point</param>
        /// <param name="fieldName"></param>
        /// <param name="distance">distance im miles</param>
        /// <param name="orderByDistance">If true, Sort results from nearest to farthest</param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public bool WithinRadius(double latitude, double longitude, string fieldName, double distance, bool orderByDistance, int maxResults)
        {
            return true;
        }
    }
}
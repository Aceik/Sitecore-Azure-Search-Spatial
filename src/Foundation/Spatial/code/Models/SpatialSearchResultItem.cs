// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGlassBase.cs" company="">
//   
// </copyright>
// <summary>
//   The GlassBase interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using Microsoft.Azure.Search;
using Newtonsoft.Json;
using Sitecore.ContentSearch;

namespace Sitecore.Foundation.CloudSpatialSearch.Models
{
    [DataContract]
    public class SpatialSearchResultItem
    {
        [IndexField("placename")]
        [DataMember]
        public virtual string PlaceName { get; set; }

        [IndexField("_fullpath")]
        [DataMember]
        public virtual string Path { get; set; }

        [IsSearchable, IsFilterable, IsSortable]
        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [IsSearchable, IsFilterable, IsSortable]
        [JsonProperty("latitude")]
        public string Latitude { get; set; }
               
        [JsonProperty("geo_location")]
        public GeoJson GeoLocation { get; set; }

        public double Distance { get; set; }
    }
}
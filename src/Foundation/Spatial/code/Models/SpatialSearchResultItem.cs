// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGlassBase.cs" company="">
//   
// </copyright>
// <summary>
//   The GlassBase interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Microsoft.Azure.Search;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace Aceik.Foundation.CloudSpatialSearch.Models
{
    [DataContract]
    public class SpatialSearchResultItem
    {
        [IndexField("_name")]
        [DataMember]
        public virtual string Name { get; set; }

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
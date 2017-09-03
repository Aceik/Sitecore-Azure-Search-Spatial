// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGlassBase.cs" company="">
//   
// </copyright>
// <summary>
//   The GlassBase interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Newtonsoft.Json;
using Microsoft.Azure.Search;

namespace Aceik.Foundation.CloudSpatialSearch.Models
{
    public class SpatialSearchResultItem
    {
        [IsSearchable, IsFilterable, IsSortable]
        [JsonProperty("fullpath_1")]
        public string FullPath { get; set; }

        [IsSearchable, IsFilterable, IsSortable]
        [JsonProperty("group_1")]
        public Guid Id { get; set; }

        [IsSearchable, IsFilterable, IsSortable]
        [JsonProperty("language_1")]
        public string Language { get; set; }

        [IsSearchable, IsFilterable, IsSortable]
        [JsonProperty("name__")]
        public string Name { get; set; }

        [IsSearchable, IsFilterable, IsSortable]
        [JsonProperty("template_1")]
        public Guid TemplateId { get; set; }

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
using Newtonsoft.Json;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace Aceik.Foundation.CloudSpatialSearch.Models
{
    public class SpatialSearchResultItem : SearchResultItem
    {
        [IndexField("geo_location")]
        public GeoJson GeoLocation { get; set; }

        public double Distance { get; set; }
    }
}
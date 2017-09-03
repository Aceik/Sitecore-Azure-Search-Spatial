using System.Collections.Generic;
using Aceik.Foundation.CloudSpatialSearch.Models;

namespace Sitecore.Feature.Maps.Models
{
    public class MapSearchResults
    {
        public MapSearchResults(List<SpatialSearchResultItem> theseResults, int timeTaken)
        {
            Results = theseResults;
            SearchMilliseconds = timeTaken;
        }

        public List<SpatialSearchResultItem> Results { get; set; }

        public int SearchMilliseconds { get; set; }
    }
}
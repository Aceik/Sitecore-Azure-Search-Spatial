using System.Collections.Generic;

namespace Sitecore.Foundation.CloudSpatialSearch.Models
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aceik.Foundation.CloudSpatialSearch.Models
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

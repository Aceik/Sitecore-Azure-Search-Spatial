using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching;
using Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching.Services;
using Geocoding;
using System.Web.Mvc;

namespace Aceik.Foundation.CloudSpatialSearch.Controller
{
    /// <inheritdoc />
    /// <summary>
    /// This controller has been setup to test the installation of this module.
    /// </summary>
    public class SpatialController : System.Web.Mvc.Controller
    {
        public ActionResult SpatialResultsTest()
        {
            IIndexConfiguration indexConfig = new IndexConfiguration();
            IDistanceCalculatorService distanceCalculator = new DistanceCalculatorService();
            ISpatialSearchService spatialSearch = new SpatialSearchService(indexConfig, distanceCalculator);
            var results = spatialSearch.GetSpatialResultsByDistance(new Location(-26.273714, 149.554685), 10000000, 100);
            return this.View("InstallationTest", results);
        }
    }
}

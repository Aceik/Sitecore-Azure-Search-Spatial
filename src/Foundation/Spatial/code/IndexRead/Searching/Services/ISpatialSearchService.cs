// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISpatialSearchService.cs" company="FLG">
//   FLG 2017
// </copyright>
// <summary>
//   Defines the ISpatialSearchService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Aceik.Foundation.AddressLookup.Models;
using Aceik.Foundation.CloudSpatialSearch.IndexRead.Core;
using Aceik.Foundation.CloudSpatialSearch.Models;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching.Services
{
    public interface ISpatialSearchService
    {
        IEnumerable<T> GetComputedByCoordinate<T>(LatLng coordinate, double settingsSearchRadius, int maxResults = 50) where T : SpatialSearchResultItem;

        List<SpatialSearchResultItem> GetSpatialResultsByDistance(LatLng coordinate, double searchRadius, int maxResults = 50);
    }
}
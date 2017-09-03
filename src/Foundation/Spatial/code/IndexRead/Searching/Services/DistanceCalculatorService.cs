// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DistanceCalculatorService.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the DistanceCalculatorService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Device.Location;
using Aceik.Foundation.AddressLookup.Models;
using Sitecore.Foundation.DependencyInjection;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching.Services
{
    [Service(typeof(IDistanceCalculatorService))]
    public class DistanceCalculatorService : IDistanceCalculatorService
    {
        public double CalculateDistanceGps(LatLng point1, LatLng point2)
        {
            var geoPoint1 = new GeoCoordinate(point1.Latitude, point1.Longitude);
            var distanceInMeters = geoPoint1.GetDistanceTo(new GeoCoordinate(point2.Latitude, point2.Longitude));
            if (distanceInMeters > 0) return distanceInMeters / 1000;
            return 0;
        }
    }
}
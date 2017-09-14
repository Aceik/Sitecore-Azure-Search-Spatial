// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DistanceCalculatorService.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the DistanceCalculatorService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Device.Location;
using Geocoding;
using Sitecore.Foundation.DependencyInjection;

namespace Sitecore.Foundation.CloudSpatialSearch.IndexRead.Searching.Services
{
    [Service(typeof(IDistanceCalculatorService))]
    public class DistanceCalculatorService : IDistanceCalculatorService
    {
        /// <summary>
        /// Calculate the distance in KM between two coordinates.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public double CalculateDistanceGps(Location point1, Location point2)
        {
            var geoPoint1 = new GeoCoordinate(point1.Latitude, point1.Longitude);
            var distanceInMeters = geoPoint1.GetDistanceTo(new GeoCoordinate(point2.Latitude, point2.Longitude));
            if (distanceInMeters > 0) return distanceInMeters / 1000;
            return 0;
        }
    }
}
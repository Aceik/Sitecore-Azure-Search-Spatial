// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDistanceCalculatorService.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the IDistanceCalculatorService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Geocoding;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching.Services
{
    public interface IDistanceCalculatorService
    {
        double CalculateDistanceGps(Location point1, Location point2);
    }
}
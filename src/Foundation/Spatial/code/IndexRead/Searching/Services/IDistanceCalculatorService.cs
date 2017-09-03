// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDistanceCalculatorService.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the IDistanceCalculatorService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Aceik.Foundation.AddressLookup.Models;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching.Services
{
    public interface IDistanceCalculatorService
    {
        double CalculateDistanceGps(LatLng point1, LatLng point2);
    }
}
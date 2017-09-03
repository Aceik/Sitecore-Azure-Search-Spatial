// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILocationSearchService.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the ILocationSearchService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Aceik.Foundation.AddressLookup.Models;

namespace Aceik.Foundation.AddressLookup.Services
{
    public interface ILocationSearchService
    {
        LatLng GetCoordinate(string location);
    }
}
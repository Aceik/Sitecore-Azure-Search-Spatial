// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGeocodeResult.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Geocode result interface
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Aceik.Foundation.AddressLookup.GeolocationSearch
{
    /// <summary>
    /// Geocode result interface
    /// </summary>
    public interface IGeocodeResult
    {
        /// <summary>
        /// Gets the locations.
        /// </summary>
        List<Location> Locations { get; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        GeocodeStatus Status { get; }
    }
}
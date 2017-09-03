// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeocodeStatus.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Geocode status enumeration
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Aceik.Foundation.AddressLookup.GeolocationSearch
{
    /// <summary>
    /// Geocode status enumeration
    /// </summary>
    public enum GeocodeStatus
    {
        OK,

        ZERO_RESULTS,

        OVER_QUERY_LIMIT,

        REQUEST_DENIED,

        INVALID_REQUEST,

        UNKNOWN_ERROR
    }
}
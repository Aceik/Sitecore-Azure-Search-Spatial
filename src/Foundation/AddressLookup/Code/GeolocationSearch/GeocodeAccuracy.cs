// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeocodeAccuracy.cs" company="Aceik">
//   Aceik 2017
// </copyright>
// <summary>
//   Geocode accuracy enumeration
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Aceik.Foundation.AddressLookup.GeolocationSearch
{
    /// <summary>
    /// Geocode accuracy enumeration
    /// </summary>
    public enum GeocodeAccuracy
    {
        UnknownLocation,

        Country,

        Region,

        SubRegion,

        Town,

        PostCode,

        Street,

        Intersection,

        Address
    }
}
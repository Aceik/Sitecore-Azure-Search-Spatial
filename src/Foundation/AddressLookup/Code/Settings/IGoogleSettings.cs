// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGoogleSettings.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the IGoogleSettings type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Aceik.Foundation.AddressLookup.Settings
{
    public interface IGoogleSettings
    {
        string ApplicationKey { get; }

        string ClientId { get; }

        string CountryPostFix { get; }

        string GeocodingEndPointUrl { get; }

        string IconMarkerUrl { get; }

        int MaxNumberResults { get; }

        double SearchRadius { get; }

        string StaticMapsWithIconMarkerUrl { get; }

        int ZoomLevel { get; }
    }
}
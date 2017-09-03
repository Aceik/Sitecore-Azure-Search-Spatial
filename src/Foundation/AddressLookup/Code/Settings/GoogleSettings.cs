// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleSettings.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the GoogleSettings type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Sitecore.Foundation.DependencyInjection;

namespace Aceik.Foundation.AddressLookup.Settings
{
    [Service(typeof(IGoogleSettings))]
    public class GoogleSettings : IGoogleSettings
    {
        public const string DefaultBaseMapLinkUrl = "https://maps.google.com/maps?daddr=";
        
        public GoogleSettings()
        {
            this.GeocodingEndPointUrl = "http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false&client={1}&channel={2}&language={3}";
            this.StaticMapsWithIconMarkerUrl = "https://maps.googleapis.com/maps/api/staticmap?center={0},{1}&zoom={2}&size={3}x{4}&maptype=roadmap&markers=icon:{5}%7C{0},{1}&sensor=false&client={6}&channel={7}";
            this.CountryPostFix = ", AU";
            this.SearchRadius = 100;
        }

        public string ApplicationKey => Sitecore.Configuration.Settings.GetSetting("Foundation.AddressLookup.Google.ApplicationKey", "notset");

        public string ClientId => Sitecore.Configuration.Settings.GetSetting("Foundation.AddressLookup.Google.ClientId", "notset");

        public string CountryPostFix { get; set; }

        public string GeocodingEndPointUrl { get; set; }

        public string IconMarkerUrl => Sitecore.Configuration.Settings.GetSetting("Foundation.AddressLookup.Google.IconMarkerUrl", "notset");

        public int MaxNumberResults => Sitecore.Configuration.Settings.GetIntSetting("Foundation.AddressLookup.Google.MaxNumberResults", 5);

        public double SearchRadius { get; set; }

        public string StaticMapsWithIconMarkerUrl { get; set; }

        public int ZoomLevel => Sitecore.Configuration.Settings.GetIntSetting("Foundation.AddressLookup.Google.ZoomLevel", 15);
    }
}
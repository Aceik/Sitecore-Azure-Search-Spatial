// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleMapUrlFactory.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the GoogleMapUrlFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using Aceik.Foundation.AddressLookup.Settings;
using Sitecore.Foundation.DependencyInjection;

namespace Aceik.Foundation.AddressLookup.Google.Factory.Maps
{
    [Service(typeof(IGoogleMapUrlFactory))]
    public class GoogleMapUrlFactory : IGoogleMapUrlFactory
    {
        private readonly IGoogleSettings googleSettings;

        private readonly IGoogleUrlSigner googleUrlSigner;

        public GoogleMapUrlFactory(IGoogleUrlSigner googleUrlSigner, IGoogleSettings googleSettings)
        {
            this.googleUrlSigner = googleUrlSigner;
            this.googleSettings = googleSettings;
        }

        public string Build(string latitude, string longitude, int width, int height)
        {
            return this.GetSignedGoogleStaticMapsWithIconMarkerUrl(latitude, longitude, width, height, "web");
        }

        private string GetSignedGoogleStaticMapsWithIconMarkerUrl(
            string latitude,
            string longitude,
            int width,
            int height,
            string channelId)
        {
            var unsignedUrl = string.Format(
                CultureInfo.InvariantCulture,
                this.googleSettings.StaticMapsWithIconMarkerUrl,
                latitude,
                longitude,
                this.googleSettings.ZoomLevel,
                width,
                height,
                this.googleSettings.IconMarkerUrl,
                this.googleSettings.ClientId,
                channelId);

            var signedUrl = this.googleUrlSigner.Sign(unsignedUrl, this.googleSettings.ApplicationKey);

            return signedUrl;
        }
    }
}
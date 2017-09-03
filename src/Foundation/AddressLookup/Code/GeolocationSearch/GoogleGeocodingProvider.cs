// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleGeocodingProvider.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the GoogleGeocodingProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Aceik.Foundation.AddressLookup.GeolocationSearch.JsonConverters;
using Aceik.Foundation.AddressLookup.Settings;

namespace Aceik.Foundation.AddressLookup.GeolocationSearch
{
    /// <summary>
    /// Google geocoding provider class
    /// </summary>
    public class GoogleGeocodingProvider
    {
        private static JavaScriptSerializer js;

        private readonly IGoogleSettings googleSettings;

        public GoogleGeocodingProvider(IGoogleSettings googleSettings)
        {
            this.googleSettings = googleSettings;
        }

        /// <summary>
        /// Gets the json serializer.
        /// </summary>
        public static JavaScriptSerializer JsonSerializer
        {
            get
            {
                if (js == null)
                {
                    js = new JavaScriptSerializer();
                    var converters = new JavaScriptConverter[]
                    {
                        new AddressConverter(), new GoogleResultConverter(), new LocationConverter(),
                    };
                    js.RegisterConverters(converters);
                }

                return js;
            }
        }

        /// <summary>
        /// Signs a URL request and output a Google Maps URL request. Note that we need to convert the default Base64 encoding to implement an URL-safe version.
        /// https://developers.google.com/maps/documentation/business/webservices/auth#digital_signatures
        /// </summary>
        /// <param name="url"></param>
        /// <param name="keyString"></param>
        /// <returns></returns>
        public static string Sign(string url, string keyString)
        {
            var encoding = new ASCIIEncoding();

            // converting key to bytes will throw an exception, need to replace '-' and '_' characters first.
            var usablePrivateKey = keyString.Replace("-", "+").Replace("_", "/");
            var privateKeyBytes = Convert.FromBase64String(usablePrivateKey);

            var uri = new Uri(url);
            var encodedPathAndQueryBytes = encoding.GetBytes(uri.LocalPath + uri.Query);

            // compute the hash
            using (var algorithm = new HMACSHA1(privateKeyBytes))
            {
                var hash = algorithm.ComputeHash(encodedPathAndQueryBytes);

                // convert the bytes to string and make url-safe by replacing '+' and '/' characters
                var signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");

                // Add the signature to the existing URI.
                return uri.Scheme + "://" + uri.Host + uri.LocalPath + uri.Query + "&signature=" + signature;
            }
        }

        /// <summary>
        /// Geocodes the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>
        /// A <see cref="IGeocodeResult"/> object.
        /// </returns>
        public IGeocodeResult Geocode(string address)
        {
            var result = new GeocodeResult(GeocodeStatus.ZERO_RESULTS);

            try
            {
                using (var webClient = new SystemWebClient())
                {
                    webClient.Proxy = WebRequest.DefaultWebProxy;
                    webClient.Encoding = Encoding.UTF8;
                    var signedUrl = this.GetSignedGoogleGeocodingUrl(address, "web");

                    var resultString = webClient.DownloadString(signedUrl);

                    result = JsonSerializer.Deserialize<GeocodeResult>(resultString);
                }
            }
            catch (WebException)
            {
                result.Status = GeocodeStatus.UNKNOWN_ERROR;
            }
            catch (NotSupportedException)
            {
                result.Status = GeocodeStatus.UNKNOWN_ERROR;
            }

            return result;
        }

        /// <summary>
        /// Geocodes the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>
        /// A <see cref="IGeocodeResult"/> object.
        /// </returns>
        public IGeocodeResult Geocode(GeoAddress address)
        {
            var addressInfo = new StringBuilder();

            if (!string.IsNullOrEmpty(address.Name))
            {
                addressInfo.Append(address.Name).Append(",");
            }

            if (!string.IsNullOrEmpty(address.Line1))
            {
                addressInfo.Append(address.Line1).Append(",");
            }

            if (!string.IsNullOrEmpty(address.Line2))
            {
                addressInfo.Append(address.Line2).Append(",");
            }

            if (!string.IsNullOrEmpty(address.City))
            {
                addressInfo.Append(address.City).Append(",");
            }

            if (!string.IsNullOrEmpty(address.County))
            {
                addressInfo.Append(address.County).Append(",");
            }

            if (!string.IsNullOrEmpty(address.Postcode))
            {
                addressInfo.Append(address.Postcode).Append(",");
            }

            if (!string.IsNullOrEmpty(address.Region))
            {
                addressInfo.Append(address.Region).Append(",");
            }

            if (!string.IsNullOrEmpty(address.County))
            {
                addressInfo.Append(address.County).Append(",");
            }

            return this.Geocode(addressInfo.ToString().TrimEnd(new[] { ',' }));
        }

        /// <summary>
        /// Gets a signed Google Maps API V3 Geocoding URL signed with the private application key.
        /// </summary>
        /// <param name="address">The address to be geocoded.</param>
        /// <param name="channelId">The channel for this request for later reporting results.</param>
        /// <returns></returns>
        public string GetSignedGoogleGeocodingUrl(string address, string channelId)
        {
            var unsignedUrl = string.Format(
                this.googleSettings.GeocodingEndPointUrl,
                HttpUtility.UrlEncode(address),
                this.googleSettings.ClientId,
                channelId,
                CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

            var signedUrl = Sign(unsignedUrl, this.googleSettings.ApplicationKey);

            return signedUrl;
        }
    }
}
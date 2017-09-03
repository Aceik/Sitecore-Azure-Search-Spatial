// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddressConverter.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Address converter class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Aceik.Foundation.AddressLookup.Models;

namespace Aceik.Foundation.AddressLookup.GeolocationSearch.JsonConverters
{
    /// <summary>
    /// Address converter class
    /// </summary>
    public class AddressConverter : JavaScriptConverter
    {
        private static readonly Type[] Types = new[] { typeof(Address) };

        /// <summary>
        /// When overridden in a derived class, gets a collection of the supported types.
        /// </summary>
        /// <returns>An object that implements <see cref="T:System.Collections.Generic.IEnumerable`1"/> that represents the types supported by the converter.</returns>
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return Types;
            }
        }

        /// <summary>
        /// When overridden in a derived class, converts the provided dictionary into an object of the specified type.
        /// </summary>
        /// <param name="dictionary">An <see cref="T:System.Collections.Generic.IDictionary`2"/> instance of property data stored as name/value pairs.</param>
        /// <param name="type">The type of the resulting object.</param>
        /// <param name="serializer">The <see cref="T:System.Web.Script.Serialization.JavaScriptSerializer"/> instance.</param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        public override object Deserialize(
            IDictionary<string, object> dictionary,
            Type type,
            JavaScriptSerializer serializer)
        {
            var country = string.Empty;
            var county = string.Empty;
            var region = string.Empty;
            var city = string.Empty;
            var postcode = string.Empty;
            var line1 = string.Empty;

            var accuracy = GeocodeAccuracy.UnknownLocation;

            var addressName = dictionary["formatted_address"] as string;

            var addressComponents = dictionary["address_components"] as ArrayList;
            if (addressComponents != null)
            {
                foreach (var addressComponent in addressComponents)
                {
                    var addressComponentDictionary = addressComponent as Dictionary<string, object>;
                    if (addressComponentDictionary != null)
                    {
                        var addressValue = addressComponentDictionary["long_name"] as string;
                        var typesArrayList = addressComponentDictionary["types"] as ArrayList;
                        if (typesArrayList != null)
                        {
                            foreach (string addressType in typesArrayList)
                            {
                                switch (addressType)
                                {
                                    case nameof(country):
                                        country = addressValue;

                                        if (accuracy < GeocodeAccuracy.Country)
                                        {
                                            accuracy = GeocodeAccuracy.Country;
                                        }

                                        break;
                                    case "administrative_area_level_1":
                                        if (string.IsNullOrEmpty(county))
                                        {
                                            county = addressValue;
                                        }
                                        else
                                        {
                                            region = county;
                                            county = addressValue;
                                        }

                                        if (accuracy < GeocodeAccuracy.Region)
                                        {
                                            accuracy = GeocodeAccuracy.Region;
                                        }

                                        break;
                                    case "administrative_area_level_2":
                                        if (string.IsNullOrWhiteSpace(county))
                                        {
                                            county = addressValue;
                                        }
                                        else
                                        {
                                            region = addressValue;
                                        }

                                        if (accuracy < GeocodeAccuracy.SubRegion)
                                        {
                                            accuracy = GeocodeAccuracy.SubRegion;
                                        }

                                        break;
                                    case "locality":
                                        city = addressValue;

                                        if (accuracy < GeocodeAccuracy.Town)
                                        {
                                            accuracy = GeocodeAccuracy.Town;
                                        }

                                        break;
                                    case "postal_town":
                                        if (string.IsNullOrWhiteSpace(city))
                                        {
                                            city = addressValue;

                                            if (accuracy < GeocodeAccuracy.Town)
                                            {
                                                accuracy = GeocodeAccuracy.Town;
                                            }
                                        }

                                        break;
                                    case "postal_code":
                                        postcode = addressValue;

                                        if (accuracy < GeocodeAccuracy.PostCode)
                                        {
                                            accuracy = GeocodeAccuracy.PostCode;
                                        }

                                        break;
                                    case "route":
                                    case "street_address":
                                        line1 = string.IsNullOrWhiteSpace(line1)
                                                    ? addressValue
                                                    : string.Format("{0} {1}", line1, addressValue);

                                        if (accuracy < GeocodeAccuracy.Street)
                                        {
                                            accuracy = GeocodeAccuracy.Street;
                                        }

                                        break;
                                    case "street_number":
                                        line1 = string.IsNullOrWhiteSpace(line1)
                                                    ? addressValue
                                                    : string.Format("{0} {1}", addressValue, line1);

                                        if (accuracy < GeocodeAccuracy.Address)
                                        {
                                            accuracy = GeocodeAccuracy.Address;
                                        }

                                        break;
                                }
                            }
                        }
                    }
                }
            }

            return new GeoAddress(
                       string.Empty,
                       line1,
                       string.Empty,
                       region,
                       county,
                       city,
                       postcode,
                       country,
                       accuracy) {
                                    FormattedAddress = addressName 
                                 };
        }

        /// <summary>
        /// When overridden in a derived class, builds a dictionary of name/value pairs.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="serializer">The object that is responsible for the serialization.</param>
        /// <returns>
        /// An object that contains key/value pairs that represent the object’s data.
        /// </returns>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
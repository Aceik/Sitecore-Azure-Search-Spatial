// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocationConverter.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the LocationConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Aceik.Foundation.AddressLookup.Models;

namespace Aceik.Foundation.AddressLookup.GeolocationSearch.JsonConverters
{
    /// <summary>
    /// Location converter class
    /// </summary>
    public class LocationConverter : JavaScriptConverter
    {
        private static readonly Type[] Types = new[] { typeof(Location) };

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
            if (dictionary != null)
            {
                var position = new LatLng(0.0, 0.0);

                if (dictionary.ContainsKey("geometry"))
                {
                    var geoLocations = dictionary["geometry"] as Dictionary<string, object>;
                    if (geoLocations != null)
                    {
                        var locationDictionary = geoLocations["location"] as Dictionary<string, object>;
                        if (locationDictionary != null)
                        {
                            position.Longitude = Convert.ToDouble(locationDictionary["lng"]);
                            position.Latitude = Convert.ToDouble(locationDictionary["lat"]);
                        }
                    }
                }

                var address = serializer.ConvertToType<GeoAddress>(dictionary);
                return new Location(address, position);
            }

            return null;
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
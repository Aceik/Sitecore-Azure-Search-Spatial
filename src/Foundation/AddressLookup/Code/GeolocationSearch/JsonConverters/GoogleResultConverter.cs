// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleResultConverter.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the GoogleResultConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace Aceik.Foundation.AddressLookup.GeolocationSearch.JsonConverters
{
    /// <summary>
    /// Google result converter class
    /// </summary>
    public class GoogleResultConverter : JavaScriptConverter
    {
        private static readonly Type[] Types = new[] { typeof(GeocodeResult) };

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
            var locations = new List<Location>();
            var status = GeocodeStatus.UNKNOWN_ERROR;
            if (Enum.TryParse(dictionary[nameof(status)].ToString(), true, out status))
            {
                var results = dictionary["results"] as ArrayList;
                if (results != null)
                {
                    locations.AddRange(from object result in results select serializer.ConvertToType<Location>(result));
                }
            }

            return new GeocodeResult(locations, status);
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
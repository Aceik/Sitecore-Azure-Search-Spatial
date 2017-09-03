// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Location.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the Location type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Aceik.Foundation.AddressLookup.Models;

namespace Aceik.Foundation.AddressLookup.GeolocationSearch
{
    /// <summary>
    /// Location result returned from a geocoding request.
    /// </summary>
    public class Location
    {
        private string error = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        public Location()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="point">The point.</param>
        public Location(GeoAddress address, LatLng point)
        {
            this.Address = address;
            this.Point = point;
        }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public GeoAddress Address { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public string Error
        {
            get
            {
                return this.error;
            }

            set
            {
                this.error = value;
            }
        }

        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        public LatLng Point { get; set; }
    }
}
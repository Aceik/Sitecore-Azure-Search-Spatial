using System.Collections.Generic;

namespace Aceik.Foundation.AddressLookup.GeolocationSearch
{
    /// <summary>
    /// Geocode result class
    /// </summary>
    public class GeocodeResult : IGeocodeResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodeResult"/> class.
        /// </summary>
        /// <param name="status">The status.</param>
        internal GeocodeResult(GeocodeStatus status)
        {
            this.Locations = new List<Location>();
            this.Status = status;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodeResult"/> class.
        /// </summary>
        /// <param name="locations">The locations.</param>
        /// <param name="status">The status.</param>
        internal GeocodeResult(IEnumerable<Location> locations, GeocodeStatus status)
        {
            this.Locations = new List<Location>(locations);
            this.Status = status;
        }

        /// <summary>
        /// Gets the locations.
        /// </summary>
        public List<Location> Locations { get; internal set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        public GeocodeStatus Status { get; internal set; }
    }
}
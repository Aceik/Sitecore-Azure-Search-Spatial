namespace Aceik.Foundation.AddressLookup.Models
{
    /// <summary>
    /// Coordinates interface
    /// </summary>
    public interface ICoordinates
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        double Longitude { get; set; }
    }
}
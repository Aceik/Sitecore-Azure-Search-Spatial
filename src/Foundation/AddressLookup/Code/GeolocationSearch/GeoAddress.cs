using System.Text;

namespace Aceik.Foundation.AddressLookup.GeolocationSearch
{
    public class GeoAddress
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoAddress"/> class.
        /// </summary>
        public GeoAddress()
        {
            this.Name = string.Empty;
            this.Line1 = string.Empty;
            this.Line2 = string.Empty;
            this.Region = string.Empty;
            this.County = string.Empty;
            this.City = string.Empty;
            this.Postcode = string.Empty;
            this.Country = string.Empty;
            this.Accuracy = GeocodeAccuracy.UnknownLocation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoAddress"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="line1">The line1.</param>
        /// <param name="line2">The line2.</param>
        /// <param name="region">The region.</param>
        /// <param name="county">The county.</param>
        /// <param name="city">The city.</param>
        /// <param name="postcode">The postcode.</param>
        /// <param name="country">The country.</param>
        /// <param name="accuracy">The accuracy.</param>
        public GeoAddress(
            string name,
            string line1,
            string line2,
            string region,
            string county,
            string city,
            string postcode,
            string country,
            GeocodeAccuracy accuracy)
        {
            this.Name = name;
            this.Line1 = line1;
            this.Line2 = line2;
            this.Region = region;
            this.County = county;
            this.City = city;
            this.Postcode = postcode;
            this.Country = country;
            this.Accuracy = accuracy;
        }

        /// <summary>
        /// Gets or sets the geocode accuracy.
        /// </summary>
        /// <value>
        /// The geocode accuracy.
        /// </value>
        public GeocodeAccuracy Accuracy { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the county.
        /// </summary>
        /// <value>
        /// The county.
        /// </value>
        public string County { get; set; }

        /// <summary>
        /// Gets or sets the formatted address.
        /// </summary>
        /// <value>
        /// The formatted address.
        /// </value>
        public string FormattedAddress { get; set; }

        /// <summary>
        /// Gets or sets the line1.
        /// </summary>
        /// <value>
        /// The line1.
        /// </value>
        public string Line1 { get; set; }

        /// <summary>
        /// Gets or sets the line2.
        /// </summary>
        /// <value>
        /// The line2.
        /// </value>
        public string Line2 { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the postcode.
        /// </summary>
        /// <value>
        /// The postcode.
        /// </value>
        public string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public string Region { get; set; }

        /// <summary>
        /// Gets the address as string.
        /// </summary>
        /// <returns>Address a string</returns>
        public string ToWebString()
        {
            if (!string.IsNullOrWhiteSpace(this.FormattedAddress))
            {
                return this.FormattedAddress;
            }

            var builder = new StringBuilder();

            if (!string.IsNullOrEmpty(this.Name)) builder.Append(this.Name + ", ");

            if (!string.IsNullOrEmpty(this.Line1)) builder.Append(this.Line1 + ", ");

            if (!string.IsNullOrEmpty(this.Line2)) builder.Append(this.Line2 + ", ");

            if (!string.IsNullOrEmpty(this.City)) builder.Append(this.City + ", ");

            if (!string.IsNullOrEmpty(this.County)) builder.Append(this.County + ", ");

            if (!string.IsNullOrEmpty(this.Postcode)) builder.Append(this.Postcode + ", ");

            if (!string.IsNullOrEmpty(this.Region)) builder.Append(this.Region + ", ");

            if (!string.IsNullOrEmpty(this.Country)) builder.Append(this.Country + ", ");

            return builder.ToString().TrimEnd(new[] { ',' });
        }
    }
}
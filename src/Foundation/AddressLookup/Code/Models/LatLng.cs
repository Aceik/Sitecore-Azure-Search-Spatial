// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LatLng.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Holds the geographic coordinates
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;

namespace Aceik.Foundation.AddressLookup.Models
{
    /// <summary>
    /// Holds the geographic coordinates
    /// </summary>
    [Serializable]
    public class LatLng
    {
        private double latitude;

        private double longitude;

        public LatLng()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatLng"/> struct.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public LatLng(double latitude, double longitude)
        {
            this.latitude = 0;
            this.longitude = 0;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatLng"/> struct.
        /// </summary>
        /// <param name="latitudeDegrees">The latitude degrees.</param>
        /// <param name="latitudeMinutes">The latitude minutes.</param>
        /// <param name="latitudeSeconds">The latitude seconds.</param>
        /// <param name="longitudeDegrees">The longitude degrees.</param>
        /// <param name="longitudeMinutes">The longitude minutes.</param>
        /// <param name="longitudeSeconds">The longitude seconds.</param>
        public LatLng(
            double latitudeDegrees,
            double latitudeMinutes,
            double latitudeSeconds,
            double longitudeDegrees,
            double longitudeMinutes,
            double longitudeSeconds)
        {
            if (latitudeDegrees > 0)
            {
                this.latitude = latitudeDegrees + (latitudeMinutes / 60) + (latitudeSeconds / 3600);
            }
            else
            {
                this.latitude = latitudeDegrees - (latitudeMinutes / 60) - (latitudeSeconds / 3600);
            }

            if (longitudeDegrees > 0)
            {
                this.longitude = longitudeDegrees + (longitudeMinutes / 60) + (longitudeSeconds / 3600);
            }
            else
            {
                this.longitude = longitudeDegrees - (longitudeMinutes / 60) - (longitudeSeconds / 3600);
            }
        }

        public static LatLng Default => new LatLng(-33.869645, 151.1924824);

        /// <summary>
        /// Gets or sets the latitude of the <see cref="LatLng"/>.
        /// </summary>
        /// <value>A <see cref="double"/> value indicating the latitude of the <see cref="LatLng"/>.</value>
        public double Latitude
        {
            get
            {
                return this.latitude;
            }

            set
            {
                if (value > 90)
                {
                    this.latitude = 90;
                }
                else if (value < -90)
                {
                    this.latitude = -90;
                }
                else
                {
                    this.latitude = value;
                }
            }
        }

        /// <summary>
        /// Gets the degrees of latitude of the <see cref="LatLng"/>.
        /// </summary>
        public int LatitudeDegrees => (int)Math.Truncate(this.Latitude);

        /// <summary>
        /// Gets the minutes of latitude of the <see cref="LatLng"/>.
        /// </summary>
        public int LatitudeMinutes
        {
            get
            {
                var d = (this.Latitude - this.LatitudeDegrees) * 60;
                return (int)Math.Abs(Math.Truncate(d));
            }
        }

        /// <summary>
        /// Gets the seconds of latitude of the <see cref="LatLng"/>.
        /// </summary>
        public double LatitudeSeconds
        {
            get
            {
                var d = (this.Latitude - this.LatitudeDegrees) * 60;
                return Math.Abs((d - this.LatitudeMinutes) * 60);
            }
        }

        /// <summary>
        /// Gets or sets the longitude of the <see cref="LatLng"/>.
        /// </summary>
        /// <value>A <see cref="double"/> value indicating the longitude of the LatLng</value>
        public double Longitude
        {
            get
            {
                return this.longitude;
            }

            set
            {
                if (value > 360)
                {
                    this.longitude = value - 360;
                }
                else if (value < -360)
                {
                    this.longitude = value + 360;
                }
                else
                {
                    this.longitude = value;
                }
            }
        }

        /// <summary>
        /// Gets the degrees of longitude of the <see cref="LatLng"/>.
        /// </summary>
        public int LongitudeDegrees => (int)Math.Truncate(this.Longitude);

        /// <summary>
        /// Gets the minutes of longitude of the <see cref="LatLng"/>.
        /// </summary>
        public int LongitudeMinutes => (int)Math.Abs(Math.Truncate((this.Longitude - this.LongitudeDegrees) * 60));

        /// <summary>
        /// Gets the seconds of longitude of the <see cref="LatLng"/>.
        /// </summary>
        public double LongitudeSeconds
        {
            get
            {
                var d = (this.Longitude - this.LongitudeDegrees) * 60;
                return Math.Abs((d - this.LongitudeMinutes) * 60);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is LatLng)
            {
                var other = (LatLng)obj;
                return this.Latitude == other.Latitude && this.Longitude == other.Longitude;
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Convert.ToInt32(this.Latitude) * 1000 + Convert.ToInt32(this.Longitude);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.ToString(string.Empty);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "C": return $"{this.Latitude.ToString(formatProvider)},{this.Longitude.ToString(formatProvider)}";

                case "D":
                    return string.Format(
                        "{0}\x00b0{1} {2}\x00b4 {3}\x00b4\x00b4 {4}\x00b0{5} {6}\x00b4 {7}\x00b4\x00b4",
                        new object[]
                            {
                                this.LatitudeDegrees, (this.LatitudeDegrees >= 0) ? "N" : "S", this.LatitudeMinutes,
                                this.LatitudeSeconds, this.LongitudeDegrees, (this.LongitudeDegrees >= 0) ? "E" : "W",
                                this.LongitudeMinutes, this.LongitudeSeconds
                            });

                case "M":
                    return
                        $"<span class=\"geo\"><span class=\"latitude\">{this.latitude.ToString(formatProvider)}</span>, <span class=\"longitude\">{this.longitude.ToString(formatProvider)}</span></span>";

                case "U":
                    return
                        $"{this.Latitude.ToString("f6", formatProvider)}/{this.Longitude.ToString("f6", formatProvider)}";
            }

            return
                $"google.maps.LatLng({this.Latitude.ToString(CultureInfo.CurrentCulture)}, {this.Longitude.ToString(CultureInfo.CurrentCulture)})";
        }
    }
}
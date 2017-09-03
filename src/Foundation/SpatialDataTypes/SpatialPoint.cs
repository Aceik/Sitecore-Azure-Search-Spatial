// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpatialPoint.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the SpatialPoint type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Foundation.Spatial.DataTypes
{
    using System;

    [Serializable]
    public class SpatialPoint
    {
        public SpatialPoint()
        {
            
        }

        public SpatialPoint(string value)
        {
            if (value == null) throw new ArgumentNullException("value");
            this.RawValue = value;
            var tokens = value.Split(',');
            if (tokens.Length != 2) throw  new ArgumentException("incorrect spatial point format. Value must be supplied in the following format lat,lon");
            var strLat = tokens[0].Trim();
            var strLon = tokens[1].Trim();
            this.Lat = double.Parse(strLat);
            this.Lon = double.Parse(strLon);
        }

        protected string RawValue { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }


        public override string ToString()
        {
            return this.RawValue;
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Templates.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the Templates type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Sitecore.Data;

namespace Sitecore.Foundation.CloudSpatialSearch
{
    public struct Templates
    {
        public struct Office
        {
            public static readonly ID ID = new ID("{7EED4C41-4F91-4EDB-AB13-1A89092BAA66}");

            public struct Fields
            {
                public static readonly ID Latitude = new ID("{ED21E7A6-C30F-4335-B007-577ECD8A7A02}");

                public static readonly ID Longitude = new ID("{022C0182-7B42-4B19-9420-087582CA2063}");
            }
        }
    }
}
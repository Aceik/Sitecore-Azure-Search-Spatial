// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoLocationField.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the GeoLocationField type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Aceik.Foundation.CloudSpatialSearch.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Aceik.Foundation.CloudSpatialSearch.IndexWrite.Computed
{
    public class GeoLocationField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        /// <summary>
        /// Based on an office location store Lat Long in GeoJson format for storage in the Azure Search Index.
        /// </summary>
        /// <param name="indexable">This indexable item.</param>
        /// <returns>A value to be stored under this field.</returns>
        public object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, $"indexable");

            var sitecoreIndexable = indexable as SitecoreIndexableItem;
            if (sitecoreIndexable?.Item == null) return null;

            if(sitecoreIndexable.Item.IsDerived(Templates.Office.ID))
            {
                var lat = sitecoreIndexable.Item.Fields[Templates.Office.Fields.Latitude].Value;
                var longitude = sitecoreIndexable.Item.Fields[Templates.Office.Fields.Longitude].Value;
                if (!string.IsNullOrWhiteSpace(lat) && !string.IsNullOrWhiteSpace(longitude))
                {
                    var geoJson = new GeoJson { Type = "Point", Coordinates = new List<double>() };

                    double latD;
                    double longitudeD;
                    if (!double.TryParse(longitude, out longitudeD) || !double.TryParse(lat, out latD))
                    {
                        // todo: log error
                        return null;
                    }

                    geoJson.Coordinates.Add(longitudeD);
                    geoJson.Coordinates.Add(latD);
                    return geoJson;
                }
            }

            return null;
        }
    }
}
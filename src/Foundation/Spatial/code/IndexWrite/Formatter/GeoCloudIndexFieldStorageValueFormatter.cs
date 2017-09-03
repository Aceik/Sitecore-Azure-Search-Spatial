// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloudIndexFieldStorageValueFormatter.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the CloudIndexFieldStorageValueFormatter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Azure;
using Sitecore.ContentSearch.Azure.Converters;

namespace Aceik.Foundation.CloudSpatialSearch.IndexWrite.Formatter
{
    /// <summary>
    /// The geo cloud index field storage value formatter.
    /// </summary>
    public class GeoCloudIndexFieldStorageValueFormatter : CloudIndexFieldStorageValueFormatter
    {
        /// <summary>
        /// The search index.
        /// </summary>
        private CloudSearchProviderIndex searchIndex;

        /// <summary>
        /// The format value for index storage.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public override object FormatValueForIndexStorage(object value, string fieldName)
        {
            object obj = value;
            if (!string.IsNullOrWhiteSpace(fieldName) && fieldName.ToLower().Contains(Sitecore.Configuration.Settings.GetSetting("Foundation.GeoSearch.Azure.GeoJsonMatcher", "geo_")))
            {
                return obj;
            }

            return base.FormatValueForIndexStorage(value, fieldName);
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="searchIndex">
        /// The search index.
        /// </param>
        public new void Initialize(ISearchIndex searchIndex)
        {
            this.searchIndex = searchIndex as CloudSearchProviderIndex;
            base.Initialize(searchIndex);
        }
    }
}
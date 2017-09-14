// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureSearchIndexWithSpatial.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the AzureSearchIndexWithSpatial type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Azure;
using Sitecore.ContentSearch.Azure.Http;
using Sitecore.ContentSearch.Azure.Schema;
using Sitecore.ContentSearch.Maintenance;

namespace Sitecore.Foundation.CloudSpatialSearch.IndexRead.Core.Provider
{
    /// <summary>
    /// The azure search index with spatial.
    /// </summary>
    public class AzureSearchIndexWithSpatial : CloudSearchProviderIndex
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Foundation.CloudSpatialSearch.IndexRead.Core.Provider.AzureSearchIndexWithSpatial" /> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="connectionStringName">
        /// The connection string name.
        /// </param>
        /// <param name="totalParallelServices">
        /// The total parallel services.
        /// </param>
        /// <param name="propertyStore">
        /// The property store.
        /// </param>
        public AzureSearchIndexWithSpatial(string name, string connectionStringName, string totalParallelServices, IIndexPropertyStore propertyStore) : this(name, connectionStringName, totalParallelServices, propertyStore, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureSearchIndexWithSpatial"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="connectionStringName">
        /// The connection string name.
        /// </param>
        /// <param name="totalParallelServices">
        /// The total parallel services.
        /// </param>
        /// <param name="propertyStore">
        /// The property store.
        /// </param>
        /// <param name="group">
        /// The group.
        /// </param>
        public AzureSearchIndexWithSpatial(string name, string connectionStringName, string totalParallelServices, IIndexPropertyStore propertyStore, string group) : base(name, connectionStringName, totalParallelServices, propertyStore, group)
        {
        }

        public new ICloudSearchIndexSchemaBuilder SchemaBuilder
        {
            get;
            set;
        }

        public new ISearchService SearchService
        {
            get;
            set;
        }

        public new ICloudSearchIndexConfiguration CloudConfiguration
        {
            get;
            set;
        }

        internal string ConnectionStringName
        {
            get;
            set;
        }

        /// <summary>
        /// The create search context.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// The <see cref="IProviderSearchContext"/>.
        /// </returns>
        public override IProviderSearchContext CreateSearchContext(Sitecore.ContentSearch.Security.SearchSecurityOptions options = Sitecore.ContentSearch.Security.SearchSecurityOptions.EnableSecurityCheck)
        {
            return new AzureSearchWithSpatialContext(this,options);
        }
    }
}

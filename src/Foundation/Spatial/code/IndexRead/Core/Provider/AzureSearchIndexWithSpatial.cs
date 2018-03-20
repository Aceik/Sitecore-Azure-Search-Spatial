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
        public AzureSearchIndexWithSpatial(string name, string connectionStringName, string totalParallelServices, IIndexPropertyStore propertyStore) : base(name, connectionStringName, totalParallelServices, propertyStore)
        {
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
            return new AzureSearchWithSpatialContext(this.ServiceCollectionClient,options);
        }
    }
}

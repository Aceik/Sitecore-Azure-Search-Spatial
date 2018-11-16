// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureSearchWithSpatialContext.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the AzureSearchWithSpatialContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using Sitecore.Abstractions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Azure;
using Sitecore.ContentSearch.Diagnostics;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Pipelines.QueryGlobalFilters;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Security;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Diagnostics;

namespace Sitecore.Foundation.CloudSpatialSearch.IndexRead.Core.Provider
{
    using ISettings = ContentSearch.Abstractions.ISettings;

    /// <summary>
    /// The azure search with spatial context.
    /// </summary>
    public class AzureSearchWithSpatialContext : CloudSearchSearchContext, IProviderSearchContext
    {
        public ServiceCollectionClient Client { get; private set; }
        /// <summary>
        /// The index.
        /// </summary>
        private readonly CloudSearchProviderIndex index;

        /// <summary>
        /// The security options.
        /// </summary>
        private readonly SearchSecurityOptions securityOptions;

        /// <summary>
        /// The content search settings.
        /// </summary>
        private readonly IContentSearchConfigurationSettings contentSearchSettings;

        /// <summary>
        /// The settings.
        /// </summary>
        private ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureSearchWithSpatialContext"/> class.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        public AzureSearchWithSpatialContext(ServiceCollectionClient serviceCollectionClient, SearchSecurityOptions options = SearchSecurityOptions.EnableSecurityCheck)
            : base(serviceCollectionClient, options)
        {
            Assert.ArgumentNotNull((object)serviceCollectionClient, "serviceCollectionClient");
            Assert.ArgumentNotNull((object)options, "options");

            Client = serviceCollectionClient;

            this.index = (CloudSearchProviderIndex)serviceCollectionClient.GetInstance<AbstractSearchIndex>(Array.Empty<object>());
            this.contentSearchSettings = this.index.Locator.GetInstance<IContentSearchConfigurationSettings>();
            this.settings = this.index.Locator.GetInstance<ISettings>();
            this.securityOptions = options;
        }

        /// <summary>
        /// The get queryable.
        /// </summary>
        /// <typeparam name="TItem">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public new IQueryable<TItem> GetQueryable<TItem>()
        {
            return this.GetQueryable<TItem>(new IExecutionContext[0]);
        }

        /// <summary>
        /// The get queryable.
        /// </summary>
        /// <param name="executionContext">
        /// The execution context.
        /// </param>
        /// <typeparam name="TItem">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public new IQueryable<TItem> GetQueryable<TItem>(IExecutionContext executionContext)
        {
            return this.GetQueryable<TItem>(new IExecutionContext[1]
              {
                executionContext
              });
        }

        /// <summary>
        /// The get queryable.
        /// </summary>
        /// <param name="executionContexts">
        /// The execution contexts.
        /// </param>
        /// <typeparam name="TItem">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public new IQueryable<TItem> GetQueryable<TItem>(params IExecutionContext[] executionContexts)
        {
            var linqToSolrIndex = new LinqToCloudIndexWithSpatial<TItem>(this, executionContexts, Client);
            if (this.contentSearchSettings.EnableSearchDebug())
                ((IHasTraceWriter)linqToSolrIndex).TraceWriter = new LoggingTraceWriter(SearchLog.Log);

            var queryable = linqToSolrIndex.GetQueryable();
            if (typeof(TItem).IsAssignableFrom(typeof(SearchResultItem)))
            {
                var globalFiltersArgs = new QueryGlobalFiltersArgs(linqToSolrIndex.GetQueryable(), typeof(TItem), executionContexts.ToList());
                this.Index.Locator.GetInstance<BaseCorePipelineManager>().Run("contentSearch.getGlobalLinqFilters", globalFiltersArgs);
                queryable = (IQueryable<TItem>)globalFiltersArgs.Query;
            }
            return queryable;
        }

    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchExtensions.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   The search extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Azure;
using Sitecore.ContentSearch.Diagnostics;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Foundation.CloudSpatialSearch.IndexRead.Core.Provider;

namespace Sitecore.Foundation.CloudSpatialSearch.IndexRead.Core
{
    /// <summary>
    /// Enable a search extension to perform a spatial linq query.
    /// </summary>
    public static  class SearchExtensions
    {
        public static IQueryable<TResult> GetExtendedQueryable<TResult>(this IProviderSearchContext context)
        {
            return GetExtendedQueryable<TResult>(context, null);
        }

        public static IQueryable<TResult> GetExtendedQueryable<TResult>(this IProviderSearchContext context, params IExecutionContext[] executionContext)
        {
            IQueryable<TResult> queryable;
            var cloudContext = context as AzureSearchWithSpatialContext;
            if (cloudContext != null)
            {
                queryable = GetCloudQueryable<TResult>(cloudContext, executionContext, cloudContext.Client);
            }
            else
            {
                throw new NotImplementedException("Current Index is not configured to use Spatial Search.");
            }
            return queryable;
        }

        private static IQueryable<TResult> GetCloudQueryable<TResult>(AzureSearchWithSpatialContext context, IExecutionContext[] executionContext, ServiceCollectionClient serviceCollectionClient)
        {
            var linqToCloudIndexWithSpatial = new LinqToCloudIndexWithSpatial<TResult>(context, executionContext, serviceCollectionClient);
            if (context.Index.Locator.GetInstance<IContentSearchConfigurationSettings>().EnableSearchDebug())
                ((IHasTraceWriter)linqToCloudIndexWithSpatial).TraceWriter = new LoggingTraceWriter(SearchLog.Log);
            return linqToCloudIndexWithSpatial.GetQueryable();
        }

    }
}
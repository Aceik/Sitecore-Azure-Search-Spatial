// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinqToCloudIndexWithSpatial.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the LinqToCloudIndexWithSpatial type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Azure;
using Sitecore.ContentSearch.Azure.Query;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Linq.Parsing;
using Sitecore.Foundation.CloudSpatialSearch.IndexRead.Core.Parsing;

namespace Sitecore.Foundation.CloudSpatialSearch.IndexRead.Core
{
    public class LinqToCloudIndexWithSpatial<TItem> : LinqToCloudIndex<TItem>
    {           
        private readonly QueryMapper<CloudQuery> queryMapper;
        private readonly QueryOptimizer<CloudQueryOptimizerState> queryOptimizer; 


        public LinqToCloudIndexWithSpatial(CloudSearchSearchContext context, IExecutionContext[] executionContexts, ServiceCollectionClient serviceCollectionClient) : base(context, executionContexts, serviceCollectionClient)
        {
            this.queryMapper = new CloudSpatialQueryMapper(this.Parameters);
            this.queryOptimizer = new SpatialCloudQueryOptimizer();
        }

        protected override QueryMapper<CloudQuery> QueryMapper
        {
            get { return this.queryMapper; }
        }

        protected override IQueryOptimizer QueryOptimizer
        {
            get { return this.queryOptimizer; }
        }


        public override IQueryable<TItem> GetQueryable()
        {
            IQueryable<TItem> queryable = new ExtendedGenericQueryable<TItem, CloudQuery>(this, this.QueryMapper, this.QueryOptimizer, this.FieldNameTranslator);
            (queryable as IHasTraceWriter).TraceWriter = ((IHasTraceWriter)this).TraceWriter;
            foreach (IPredefinedQueryAttribute predefinedQueryAttribute in Enumerable.ToList(Enumerable.SelectMany(this.GetTypeInheritance(typeof(TItem)), t => Enumerable.Cast<IPredefinedQueryAttribute>(t.GetCustomAttributes(typeof(IPredefinedQueryAttribute), true)))))
                queryable = predefinedQueryAttribute.ApplyFilter<TItem>(queryable, this.ValueFormatter);
            return queryable;
        }

        private IEnumerable<Type> GetTypeInheritance(Type type)
        {
            yield return type;
            for (Type baseType = type.BaseType; baseType != (Type)null; baseType = baseType.BaseType)
                yield return baseType;
        }
    }
}
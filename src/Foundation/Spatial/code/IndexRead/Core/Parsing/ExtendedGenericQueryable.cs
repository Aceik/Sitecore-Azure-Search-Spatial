using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Aceik.Foundation.CloudSpatialSearch.IndexRead.Common;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Linq.Indexing;
using Sitecore.ContentSearch.Linq.Parsing;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Core.Parsing
{
    /// <summary>
    /// The extended generic queryable.
    /// </summary>
    /// <typeparam name="TElement">
    /// </typeparam>
    /// <typeparam name="TQuery">
    /// </typeparam>
    public class ExtendedGenericQueryable<TElement, TQuery> : GenericQueryable<TElement, TQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedGenericQueryable{TElement,TQuery}"/> class.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="queryMapper">
        /// The query mapper.
        /// </param>
        /// <param name="queryOptimizer">
        /// The query optimizer.
        /// </param>
        /// <param name="fieldNameTranslator">
        /// The field name translator.
        /// </param>
        public ExtendedGenericQueryable(Index<TElement, TQuery> index, QueryMapper<TQuery> queryMapper, IQueryOptimizer queryOptimizer, FieldNameTranslator fieldNameTranslator) : 
            base(index, queryMapper, queryOptimizer, fieldNameTranslator)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedGenericQueryable{TElement,TQuery}"/> class.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="queryMapper">
        /// The query mapper.
        /// </param>
        /// <param name="queryOptimizer">
        /// The query optimizer.
        /// </param>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="fieldNameTranslator">
        /// The field name translator.
        /// </param>
        protected ExtendedGenericQueryable(Index<TQuery> index, QueryMapper<TQuery> queryMapper, IQueryOptimizer queryOptimizer, Expression expression, Type itemType, FieldNameTranslator fieldNameTranslator) : 
            base(index, queryMapper, queryOptimizer, expression, itemType, fieldNameTranslator)
        {
        }

        /// <summary>
        /// The create query.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <typeparam name="TElement">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1622:GenericTypeParameterDocumentationMustHaveText", Justification = "Reviewed. Suppression is OK here.")]
#pragma warning disable 693
        public override IQueryable<TElement> CreateQuery<TElement>(Expression expression)
#pragma warning restore 693
        {
            var genericQueryable = new ExtendedGenericQueryable<TElement, TQuery>(this.Index, this.QueryMapper, this.QueryOptimizer, expression, this.ItemType, this.FieldNameTranslator);
            ((IHasTraceWriter)genericQueryable).TraceWriter = ((IHasTraceWriter)this).TraceWriter;
            return genericQueryable;
        }

        /// <summary>
        /// The get query.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// The <see cref="TQuery"/>.
        /// </returns>
        protected override TQuery GetQuery(Expression expression)
        {
            this.Trace(expression, "Expression");
            IndexQuery indexQuery = new ExpressionParserWithSpatial(typeof(TElement), this.ItemType, this.FieldNameTranslator).Parse(expression);
            this.Trace(indexQuery, "Raw query:");
            IndexQuery optimizedQuery = this.QueryOptimizer.Optimize(indexQuery);
            this.Trace(optimizedQuery, "Optimized query:");
            TQuery nativeQuery = this.QueryMapper.MapQuery(optimizedQuery);
            this.Trace(new GenericDumpable((object)nativeQuery), "Native query:");
            return nativeQuery;
        }
    }
}
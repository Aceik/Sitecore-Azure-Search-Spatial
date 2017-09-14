// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpatialCloudQueryOptimizer.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the SpatialCloudQueryOptimizer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Sitecore.ContentSearch.Azure.Query;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.Foundation.CloudSpatialSearch.IndexRead.Core.Nodes;

namespace Sitecore.Foundation.CloudSpatialSearch.IndexRead.Core
{
    /// <summary>
    /// The spatial cloud query optimizer.
    /// </summary>
    public class SpatialCloudQueryOptimizer : CloudQueryOptimizer
    {
        /// <summary>
        /// The visit.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="QueryNode"/>.
        /// </returns>
        protected override QueryNode Visit(QueryNode node, CloudQueryOptimizerState state)
        {
            if (node.NodeType == QueryNodeType.Custom)
            {
                if (node is WithinRadiusNode)
                {
                    return this.VisitWithinRadius((WithinRadiusNode)node, state);
                }
            }
           
            return base.Visit(node, state);
        }

        /// <summary>
        /// The visit within radius.
        /// </summary>
        /// <param name="radiusNode">
        /// The radius node.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="QueryNode"/>.
        /// </returns>
        private QueryNode VisitWithinRadius(WithinRadiusNode radiusNode, CloudQueryOptimizerState state)
        {
            return new WithinRadiusNode(radiusNode.Latitude, radiusNode.Longitude, radiusNode.FieldName, radiusNode.Radius, radiusNode.DistanceSort, radiusNode.MaxResults);
        }
    }
}
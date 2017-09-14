// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloudSpatialQueryMapper.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the CloudSpatialQueryMapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using Sitecore.ContentSearch.Azure.Query;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.Foundation.CloudSpatialSearch.IndexRead.Core.Nodes;

namespace Sitecore.Foundation.CloudSpatialSearch.IndexRead.Core
{
    public class CloudSpatialQueryMapper : CloudQueryMapper
    {
        public CloudSpatialQueryMapper(CloudIndexParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        /// Overrides the base   HandleCloudQuery so that we can support a spatial search VisitWithinRadius.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mappingState"></param>
        /// <returns></returns>
        protected override string HandleCloudQuery(QueryNode node, CloudQueryMapper.CloudQueryMapperState mappingState)
        {
            if (node.NodeType == QueryNodeType.Custom)
            {
                if (node is WithinRadiusNode)
                {
                    return this.VisitWithinRadius((WithinRadiusNode)node, mappingState);
                }
            }
            return base.HandleCloudQuery(node, mappingState);
        }

        /// <summary>
        /// Provides a Geo Spatial Search Query linq provider.
        /// </summary>
        /// <param name="radiusNode"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected virtual string VisitWithinRadius(WithinRadiusNode radiusNode, CloudQueryMapper.CloudQueryMapperState state)
        {
            var orderBy = string.Empty;
            if((bool)radiusNode.DistanceSort)
                orderBy = $"&$orderby=geo.distance({radiusNode.FieldName}, geography'POINT({radiusNode.Longitude} {radiusNode.Latitude})') asc";

            var queryText = $"&search=*&$top={radiusNode.MaxResults}&$filter=geo.distance({radiusNode.FieldName}, geography'POINT({radiusNode.Longitude} {radiusNode.Latitude})') le {radiusNode.Radius}{orderBy}";

            return queryText;
        }
    }


}
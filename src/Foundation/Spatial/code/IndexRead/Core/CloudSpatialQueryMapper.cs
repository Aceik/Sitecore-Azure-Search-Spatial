// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloudSpatialQueryMapper.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the CloudSpatialQueryMapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Collections.Generic;
using Aceik.Foundation.CloudSpatialSearch.IndexRead.Core.Nodes;
using Sitecore.ContentSearch.Azure.Query;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Linq.Methods;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.ContentSearch.Linq.Parsing;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Core
{
    public class CloudSpatialQueryMapper : CloudQueryMapper
    {
        public CloudSpatialQueryMapper(CloudIndexParameters parameters) : base(parameters)
        {
        }

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
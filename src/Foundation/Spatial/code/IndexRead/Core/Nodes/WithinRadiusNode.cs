// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WithinRadiusNode.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the WithinRadiusNode type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Sitecore.ContentSearch.Linq.Nodes;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Core.Nodes
{
    public class WithinRadiusNode : QueryNode
    {
        public float Boost { get; protected set; }
        public string Field { get; protected set; }
        public object FieldName { get; protected set; }
        public object Latitude { get; protected set; }
        public object Longitude { get; protected set; }
        public object Radius { get; protected set; }

        public object MaxResults { get; protected set; }
        public object DistanceSort { get; protected set; }

        public QueryNode SourceNode
        {
            get;
            protected set;
        }
              
        public override QueryNodeType NodeType
        {
            get { return QueryNodeType.Custom; }
        }

        public override IEnumerable<QueryNode> SubNodes
        {
            get
            {
                return new List<QueryNode>();

            }
        }

        public WithinRadiusNode(object lat, object lng, object name, object withinRadiusinMiles, object distanceSort, object max) : this(lat, lng, name, withinRadiusinMiles, 1f, distanceSort, max)
        {

        }

        public WithinRadiusNode(object lat, object lng, object name, object withinRadiusinMiles, float boost, object distanceSort, object maxResults)
        {
            Latitude = lat;
            Longitude = lng;
            Radius = withinRadiusinMiles;
            MaxResults = maxResults;
            Boost = boost;
            DistanceSort = distanceSort;
            FieldName = name;
        }
    }
}
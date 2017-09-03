// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendedExpressionParser.cs" company="Aceik">
//   
// </copyright>
// <summary>
//   Defines the ExtendedExpressionParser type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Reflection;
using Aceik.Foundation.CloudSpatialSearch.IndexRead.Core.Nodes;
using Aceik.Foundation.CloudSpatialSearch.Models;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.ContentSearch.Linq.Parsing;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Core.Parsing
{
    /// <summary>
    /// The extended expression parser.
    /// </summary>
    public class ExpressionParserWithSpatial : ExpressionParser
    {
        public ExpressionParserWithSpatial(Type elementType, Type itemType, FieldNameTranslator fieldNameTranslator)
            : base(elementType, itemType, fieldNameTranslator) { }

        protected override QueryNode VisitMethodCall(MethodCallExpression methodCall)
        {
            MethodInfo method = methodCall.Method;
            if (method.DeclaringType == typeof(GeoJson))
            {
                return this.VisitLocationPointMethods(methodCall);
            }
            return base.VisitMethodCall(methodCall);

        }

        protected QueryNode VisitLocationPointMethods(MethodCallExpression methodCall)
        {
            switch (methodCall.Method.Name)
            {
                case "WithinRadius":
                    return VisitWithinRadiusMethod(methodCall);

            }
            throw new NotSupportedException(string.Format("Unsupported extension method: {0}.", methodCall.Method.Name));
        }

        protected QueryNode VisitWithinRadiusMethod(MethodCallExpression methodCall)
        {
            QueryNode nodeLatitude = this.Visit(this.GetArgument(methodCall.Arguments, 0));
            QueryNode nodeLongitude = this.Visit(this.GetArgument(methodCall.Arguments, 1));
            QueryNode fieldName = this.Visit(this.GetArgument(methodCall.Arguments, 2));
            QueryNode nodeRadius = this.Visit(this.GetArgument(methodCall.Arguments, 3));
            QueryNode nodesortByDistance = this.Visit(this.GetArgument(methodCall.Arguments, 4));
            QueryNode maxResults = this.Visit(this.GetArgument(methodCall.Arguments, 5));
            object sortByDistanceValue = this.GetConstantValue<object>(nodesortByDistance);

            string fieldN = this.GetConstantValue<string>(fieldName);
            object latitudeValue = this.GetConstantValue<object>(nodeLatitude);
            object longitudeValue = this.GetConstantValue<object>(nodeLongitude);
            object radiusValue = this.GetConstantValue<object>(nodeRadius);
            object maxValue = this.GetConstantValue<object>(maxResults);

            return new WithinRadiusNode(latitudeValue, longitudeValue, fieldN, radiusValue, sortByDistanceValue, maxValue);
        }
    }
}
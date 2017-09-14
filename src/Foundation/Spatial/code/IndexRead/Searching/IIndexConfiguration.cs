// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexConfiguration.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the IIndexConfiguration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Foundation.CloudSpatialSearch.IndexRead.Searching
{
    public interface IIndexConfiguration
    {
        string AzureSearchConnectionString { get; }

        string IndexName { get; }
    }
}
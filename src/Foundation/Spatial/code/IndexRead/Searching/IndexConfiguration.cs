// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexingConfiguration.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the IndexConfiguration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Sitecore.Foundation.DependencyInjection;

namespace Aceik.Foundation.CloudSpatialSearch.IndexRead.Searching
{
    [Service(typeof(IIndexConfiguration))]
    public class IndexConfiguration : IIndexConfiguration
    {
        public string AzureSearchConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["cloud.search"].ConnectionString;

        public string IndexName => Sitecore.Configuration.Settings.GetSetting("developerName") + "{0}_geo_index";
    }
}
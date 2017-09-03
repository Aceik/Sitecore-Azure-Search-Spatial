// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MapPointRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the MapPointRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Feature.Maps.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using global::Sitecore.Foundation.DependencyInjection;

    /// <summary>
    /// The map point repository.
    /// </summary>
    [Service(typeof(IMapPointRepository))]
    public class MapPointRepository : IMapPointRepository
    {
        public IEnumerable<MapPoint> GetAll(Data.Items.Item contextItem)
        {
            if (contextItem == null)
            {
                throw new ArgumentNullException(nameof(contextItem));
            }
            if (Foundation.SitecoreExtensions.Extensions.ItemExtensions.IsDerived(contextItem, Templates.MapPoint.ID))
            {
                return new List<MapPoint>
                {
                    new MapPoint(contextItem)
                };
            }
            //if (!Foundation.SitecoreExtensions.Extensions.ItemExtensions.IsDerived(contextItem, Templates.MapPointsFolder.ID))
            //{
            //    throw new ArgumentException("Item must derive from MapPointsFolder or MapPoint", nameof(contextItem));
            //}

            //var searchService = this.searchServiceRepository.Get(new Foundation.Indexing.Models.SearchSettingsBase { Templates = new[] { Templates.MapPoint.ID } });
            //searchService.Settings.Root = contextItem;

            //return searchService.FindAll().Results.Select(x => new MapPoint(x.Item));
            return new List<MapPoint>
            {
                new MapPoint(contextItem)
            };
        }
    }
}
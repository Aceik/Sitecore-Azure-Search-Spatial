// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MvcControllerServicesConfigurator.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the MvcControllerServicesConfigurator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Foundation.DependencyInjection.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    /// <summary>
    /// The mvc controller services configurator.
    /// </summary>
    public class MvcControllerServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {                                                                      
            serviceCollection.AddMvcControllers("*.Feature.*");
            serviceCollection.AddClassesWithServiceAttribute("*.Feature.*");
            serviceCollection.AddClassesWithServiceAttribute("*.Foundation.*");
        }
    }
}
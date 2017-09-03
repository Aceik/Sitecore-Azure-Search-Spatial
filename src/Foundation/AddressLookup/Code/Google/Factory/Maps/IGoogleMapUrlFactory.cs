// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGoogleMapUrlFactory.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the IGoogleMapUrlFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Aceik.Foundation.AddressLookup.Google.Factory.Maps
{
    public interface IGoogleMapUrlFactory
    {
        string Build(string latitude, string longitude, int width, int height);
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGoogleUrlSigner.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the IGoogleUrlSigner type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Aceik.Foundation.AddressLookup.Google.Factory
{
    public interface IGoogleUrlSigner
    {
        string Sign(string url, string keyString);
    }
}
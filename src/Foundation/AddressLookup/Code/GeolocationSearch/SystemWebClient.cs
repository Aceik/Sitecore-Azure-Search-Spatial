// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemWebClient.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the SystemWebClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.Text;

namespace Aceik.Foundation.AddressLookup.GeolocationSearch
{
    public interface IWebClient : IDisposable
    {
        Encoding Encoding { get; set; }

        IWebProxy Proxy { get; set; }

        byte[] DownloadData(Uri address);

        string DownloadString(string address);

        byte[] UploadData(Uri address, byte[] data);
    }

    public class SystemWebClient : WebClient, IWebClient
    {
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Address.cs" company="Aceik">
//  Aceik
// </copyright>
// <summary>
//   Defines the Address type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Aceik.Foundation.AddressLookup.Models
{
    /// <summary>
    /// The address.
    /// </summary>
    public class Address : IAddress
    {
        public Address(
            string addressLine1,
            string addressLine2,
            string city,
            string postcode,
            string premiseName,
            string state)
        {
            this.AddressLine1 = addressLine1;
            this.AddressLine2 = addressLine2;
            this.City = city;
            this.Postcode = postcode;
            this.PremiseName = premiseName;
            this.State = state;
        }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        public string PremiseName { get; set; }

        public string State { get; set; }
    }
}
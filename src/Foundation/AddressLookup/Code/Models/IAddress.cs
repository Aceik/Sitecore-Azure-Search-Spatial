namespace Aceik.Foundation.AddressLookup.Models
{
    public interface IAddress
    {
        string AddressLine1 { get; }

        string AddressLine2 { get; }

        string City { get; }

        string Postcode { get; }

        string PremiseName { get; }

        string State { get; }
    }
}
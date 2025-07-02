namespace OrdersProjectDorisAquino.Domain.Interfaces.ExternalServices;

public interface IAddressValidationService
{
    Task<AddressValidationResult> ValidateAddressAsync(
        string addressLine1,
        string addressLine2,
        string locality,
        string administrativeArea,
        string postalCode,
        string countryCode);
}

public record AddressValidationResult
{
    public bool IsValid { get; init; }
    public string? StandardizedAddress { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public string? ValidationMessage { get; init; }
}
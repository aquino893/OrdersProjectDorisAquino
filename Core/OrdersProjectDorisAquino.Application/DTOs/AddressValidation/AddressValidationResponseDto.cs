namespace OrdersProjectDorisAquino.Application.DTOs.AddressValidation;

public class AddressValidationResponseDto
{
    public bool IsValid { get; set; }
    public string? StandardizedAddress { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? ValidationMessage { get; set; }
    public Dictionary<string, string>? AddressComponents { get; set; }
}

public class AddressComponentDto
{
    public string ComponentType { get; set; }
    public string ComponentName { get; set; }
}
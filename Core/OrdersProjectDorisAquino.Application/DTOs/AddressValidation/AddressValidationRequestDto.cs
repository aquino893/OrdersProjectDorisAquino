namespace OrdersProjectDorisAquino.Application.DTOs.AddressValidation;

public class AddressValidationRequestDto
{
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string Locality { get; set; } = string.Empty;
    public string AdministrativeArea { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
}
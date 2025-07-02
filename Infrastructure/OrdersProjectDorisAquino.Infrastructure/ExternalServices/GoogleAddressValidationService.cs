using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrdersProjectDorisAquino.Domain.Interfaces.ExternalServices;

namespace OrdersProjectDorisAquino.Infrastructure.ExternalServices;

public class GoogleAddressValidationService : IAddressValidationService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<GoogleAddressValidationService> _logger;
    private const string BaseEndpoint = "v1/address:validateAddress"; // Endpoint fijo

    public GoogleAddressValidationService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<GoogleAddressValidationService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GoogleMaps:ApiKey"];
        _logger = logger;
    }

    public async Task<AddressValidationResult> ValidateAddressAsync(
        string addressLine1,
        string addressLine2,
        string locality,
        string administrativeArea,
        string postalCode,
        string countryCode)
    {
        try
        {
            // 1. Construcción del request
            var request = new
            {
                address = new
                {
                    addressLines = new[] 
                    {
                        addressLine1?.Trim() ?? "",
                        addressLine2?.Trim() ?? ""
                    },
                    regionCode = countryCode?.Trim() ?? "",
                    locality = !string.IsNullOrWhiteSpace(locality) ? locality.Trim() : null,
                    administrativeArea = !string.IsNullOrWhiteSpace(administrativeArea) ? administrativeArea.Trim() : null,
                    postalCode = !string.IsNullOrWhiteSpace(postalCode) ? postalCode.Trim() : null
                }
            };

            // 2. Log del request (para debug)
            _logger.LogInformation("Request to Google: {Request}", JsonSerializer.Serialize(request));

            // 3. Envío a Google
            var response = await _httpClient.PostAsJsonAsync(
                $"{BaseEndpoint}?key={_apiKey}", 
                request);

            // 4. Manejo de errores HTTP
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Google API error: {StatusCode} - {Content}", 
                    response.StatusCode, errorContent);
                
                return new AddressValidationResult
                {
                    IsValid = false,
                    ValidationMessage = $"Google API error: {response.StatusCode}"
                };
            }

            // 5. Procesamiento de respuesta exitosa
            var responseContent = await response.Content.ReadAsStringAsync();
            var googleResponse = JsonSerializer.Deserialize<GoogleValidationResponse>(responseContent);

            return new AddressValidationResult
            {
                IsValid = googleResponse?.Result?.Verdict?.HasUnconfirmedComponents == false,
                StandardizedAddress = googleResponse?.Result?.Address?.FormattedAddress,
                Latitude = googleResponse?.Result?.Geocode?.Location?.Latitude,
                Longitude = googleResponse?.Result?.Geocode?.Location?.Longitude,
                ValidationMessage = googleResponse?.Result?.Verdict?.ValidationGranularity ?? "Validación exitosa"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en validación de dirección");
            return new AddressValidationResult
            {
                IsValid = false,
                ValidationMessage = $"Error interno: {ex.Message}"
            };
        }
    }

    // Clases para deserialización de la respuesta de Google
    private class GoogleValidationResponse
    {
        public ValidationResult? Result { get; set; }
    }

    private class ValidationResult
    {
        public ValidatedAddress? Address { get; set; }
        public AddressVerdict? Verdict { get; set; }
        public Geocode? Geocode { get; set; }
    }

    private class ValidatedAddress
    {
        public string? FormattedAddress { get; set; }
    }

    private class AddressVerdict
    {
        public bool HasUnconfirmedComponents { get; set; }
        public string? ValidationGranularity { get; set; }
    }

    private class Geocode
    {
        public GeoLocation? Location { get; set; }
    }

    private class GeoLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
// API/Controllers/AddressValidationController.cs
using Microsoft.AspNetCore.Mvc;
using OrdersProjectDorisAquino.Application.DTOs.AddressValidation;
using OrdersProjectDorisAquino.Domain.Interfaces.ExternalServices;

namespace OrdersProjectDorisAquino.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressValidationController : ControllerBase
{
    private readonly IAddressValidationService _validationService;
    private readonly ILogger<AddressValidationController> _logger;

    public AddressValidationController(
        IAddressValidationService validationService,
        ILogger<AddressValidationController> logger)
    {
        _validationService = validationService;
        _logger = logger;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateAddress([FromBody] AddressValidationRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validationResult = await _validationService.ValidateAddressAsync(
                request.AddressLine1,
                request.AddressLine2,
                request.Locality,
                request.AdministrativeArea,
                request.PostalCode,
                request.CountryCode);

            return Ok(new AddressValidationResponseDto
            {
                IsValid = validationResult.IsValid,
                StandardizedAddress = validationResult.StandardizedAddress,
                Latitude = validationResult.Latitude,
                Longitude = validationResult.Longitude,
                ValidationMessage = validationResult.ValidationMessage
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating address");
            return StatusCode(500, new AddressValidationResponseDto
            {
                IsValid = false,
                ValidationMessage = "Internal server error during address validation"
            });
        }
    }
}
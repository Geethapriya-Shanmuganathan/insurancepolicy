using InsurancePolicyMS.DTOs;
using InsurancePolicyMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePolicyMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PremiumCalculationController : Controller
    {
        private readonly IPremiumCalculationService _premiumService;

        public PremiumCalculationController(IPremiumCalculationService premiumService)
        {
            _premiumService = premiumService;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculatePremium([FromBody] PremiumCalculationDto requestDto)
        {
            // Validate the incoming DTO based on its data annotations
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns validation errors to the client
            }

            try
            {
                // Call your service method with the IDs from the DTO
                var calculation = await _premiumService.CalculatePremium(requestDto.PolicyId, requestDto.CustomerId);

                // Return the calculated premium details.
                // You might also consider returning a DTO for the output if you want to
                // exclude navigation properties from the response as well.
                return Ok(calculation);
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific business logic errors
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors
                return StatusCode(500, "An error occurred during premium calculation: " + ex.Message);
            }
        }
    }
}

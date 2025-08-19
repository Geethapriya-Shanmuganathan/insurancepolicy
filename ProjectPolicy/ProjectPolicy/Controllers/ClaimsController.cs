using InsurancePolicyMS.DTOs;
using InsurancePolicyMS.Models;
using InsurancePolicyMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePolicyMS.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimsService _claimsService;

        public ClaimsController(IClaimsService claimsService)

        {
            _claimsService = claimsService;

        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim([FromBody] ClaimDto claimDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the DTO to the entity model
            var claim = new Claim
            {
                PolicyId = claimDto.PolicyId,
                ClaimAmount = claimDto.ClaimAmount,
                ClaimStatus = claimDto.ClaimStatus, // Will be PENDING by default
                SubmissionDate = claimDto.SubmissionDate,
            };

            var submittedClaim = await _claimsService.SubmitClaim(claim);

            // Using nameof to ensure the action name is correct
            return CreatedAtAction(nameof(GetClaimDetails), new { claimId = submittedClaim.ClaimId }, submittedClaim);
        }

        [HttpPut("{claimId}/process")]

        public async Task<IActionResult> ProcessClaim(int claimId, [FromQuery] ClaimStatus status)

        {

            var processedClaim = await _claimsService.ProcessClaim(claimId, status);

            if (processedClaim == null)

            {

                return NotFound();

            }

            return Ok(processedClaim);

        }

        [HttpGet("{claimId}")]

        public async Task<IActionResult> GetClaimDetails(int claimId)

        {

            var claim = await _claimsService.GetClaimDetails(claimId);

            if (claim == null)

            {

                return NotFound();

            }

            return Ok(claim);

        }
        [HttpGet("report/{policyId}")]
        public async Task<IActionResult> GetClaimReportByPolicyId(int policyId)
        {
            var report = await _claimsService.GetClaimReport(policyId);

            if (report == null || !report.Any())
            {
                return NotFound($"No claims found for PolicyId {policyId}");
            }

            return Ok(report);
        }

    }

}
using InsurancePolicyMS.DTOs;
using InsurancePolicyMS.Models;
using InsurancePolicyMS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InsurancePolicyMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;

        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        [HttpPost]

        public async Task<IActionResult> CreatePolicy([FromBody] PolicyDto policyDto)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var policy = new Policy
            {
                PolicyType = policyDto.PolicyType,
                CoverageAmount = policyDto.CoverageAmount,
                PremiumAmount = policyDto.PremiumAmount,
                ValidityStartDate = policyDto.ValidityStartDate,
                ValidityEndDate = policyDto.ValidityEndDate
                // Collections are not set here
            };

            var result = await _policyService.CreatePolicyAsync(policy);
            return CreatedAtAction(nameof(GetPolicyDetails), new { policyId = result.PolicyId }, result);
        }

            [HttpPut("{policyId}")]
        public async Task<IActionResult> UpdatePolicy(int policyId, [FromBody] Policy policy)
        {
            var result = await _policyService.UpdatePolicyAsync(policyId, policy);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("{policyId}")]
        public async Task<IActionResult> GetPolicyDetails(int policyId)
        {
            var result = await _policyService.GetPolicyDetailsAsync(policyId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{policyId}")]
        public async Task<IActionResult> DeletePolicy(int policyId)
        {
            var success = await _policyService.DeletePolicyAsync(policyId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPolicies()
        {
            var result = await _policyService.GetAllPoliciesAsync();
            return Ok(result);
        }
    }
}
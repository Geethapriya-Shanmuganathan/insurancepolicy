using InsurancePolicyMS.DTOs;
using InsurancePolicyMS.Models;

namespace InsurancePolicyMS.Services
{
    public interface IClaimsService
    {
        Task<Claim> SubmitClaim(Claim claim);
        Task<Claim> ProcessClaim(int claimId, ClaimStatus status);
        Task<Claim> GetClaimDetails(int claimId);
        Task<List<ClaimReport>> GetClaimReport(int policyId);

    }
}

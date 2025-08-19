using InsurancePolicyMS.DTOs;
using InsurancePolicyMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InsurancePolicyMS.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly InsuranceDbContext _context;

        public ClaimsService(InsuranceDbContext context)

        {

            _context = context;

        }

        public async Task<Claim> SubmitClaim(Claim claim)

        {

            // Business rule: Validate claim against policy terms.

            // This is a placeholder for more complex logic.

            var policy = await _context.Policies.FindAsync(claim.PolicyId);

            if (policy == null)

            {

                throw new InvalidOperationException("Policy not found.");

            }

            if (claim.ClaimAmount > policy.CoverageAmount)

            {

                throw new InvalidOperationException("Claim amount exceeds policy coverage.");

            }

            claim.ClaimStatus = ClaimStatus.PENDING;

            claim.SubmissionDate = DateTime.Now;

            _context.Claims.Add(claim);

            await _context.SaveChangesAsync();

            return claim;

        }

        public async Task<Claim> ProcessClaim(int claimId, ClaimStatus status)

        {

            var claim = await _context.Claims.FindAsync(claimId);

            if (claim == null)

            {

                return null;

            }

            // Only allow processing if the claim is pending

            if (claim.ClaimStatus != ClaimStatus.PENDING)

            {

                throw new InvalidOperationException("Only pending claims can be processed.");

            }

            claim.ClaimStatus = status;

            claim.SettlementDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return claim;

        }

        public async Task<Claim> GetClaimDetails(int claimId)

        {

            return await _context.Claims
                   .Include(c => c.Policy)
                   .FirstOrDefaultAsync(c => c.ClaimId == claimId);

        }
        public async Task<List<ClaimReport>> GetClaimReport(int policyId)
        {
            var claims = await _context.Claims
                .Include(c => c.Policy)
                .Where(c => c.PolicyId == policyId)
                .Select(c => new ClaimReport
                {
                    PolicyId = c.PolicyId,
                    ClaimId = c.ClaimId,
                    PolicyType = c.Policy.PolicyType,
                    CoverageAmount = c.Policy.CoverageAmount,
                    ClaimAmount = c.ClaimAmount
                })
                .ToListAsync();

            return claims;
        }


    }


}


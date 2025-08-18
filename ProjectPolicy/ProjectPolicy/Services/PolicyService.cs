using InsurancePolicyMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsurancePolicyMS.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly InsuranceDbContext _context;

        public PolicyService(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<Policy> CreatePolicyAsync(Policy policy)
        {
            _context.Policies.Add(policy);
            await _context.SaveChangesAsync();
            return policy;
        }

        public async Task<Policy> UpdatePolicyAsync(int policyId, Policy updatedPolicy)
        {
            var existing = await _context.Policies.FindAsync(policyId);
            if (existing == null) return null;

            existing.PolicyType = updatedPolicy.PolicyType;
            existing.CoverageAmount = updatedPolicy.CoverageAmount;
            existing.PremiumAmount = updatedPolicy.PremiumAmount;
            existing.ValidityStartDate = updatedPolicy.ValidityStartDate;
            existing.ValidityEndDate = updatedPolicy.ValidityEndDate;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Policy> GetPolicyDetailsAsync(int policyId)
        {
            return await _context.Policies.FindAsync(policyId);
        }

        public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
        {
            return await _context.Policies.ToListAsync();
        }

        public async Task<bool> DeletePolicyAsync(int policyId)
        {
            var policy = await _context.Policies.FindAsync(policyId);
            if (policy == null) return false;

            _context.Policies.Remove(policy);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

using InsurancePolicyMS.Models;

namespace InsurancePolicyMS.Services
{
    public interface IPolicyService
    {
        Task<Policy> CreatePolicyAsync(Policy policy);
        Task<Policy> UpdatePolicyAsync(int policyId, Policy updatedPolicy);
        Task<Policy> GetPolicyDetailsAsync(int policyId);
        Task<bool> DeletePolicyAsync(int policyId);
        Task<IEnumerable<Policy>> GetAllPoliciesAsync();
    }
}

using InsurancePolicyMS.Models;

namespace InsurancePolicyMS.Services
{
    public interface IPremiumCalculationService
    {
        Task<PremiumCalculation> CalculatePremium(int policyId, int customerId);
    }
}

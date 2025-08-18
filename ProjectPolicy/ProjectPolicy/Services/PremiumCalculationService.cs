using InsurancePolicyMS.Models;

namespace InsurancePolicyMS.Services
{
    public class PremiumCalculationService : IPremiumCalculationService
    {
        private readonly InsuranceDbContext _context;

        public PremiumCalculationService(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<PremiumCalculation> CalculatePremium(int policyId, int customerId)
        {
            var policy = await _context.Policies.FindAsync(policyId);
            var customer = await _context.Customers.FindAsync(customerId);

            if (policy == null || customer == null)
            {
                throw new InvalidOperationException("Policy or Customer not found.");
            }

            // Placeholder for a premium calculation algorithm.
            // This logic would be much more complex in a real-world scenario.
            // Factors could include customer age, policy type, coverage amount, etc.
            decimal basePremium = policy.PremiumAmount;
            decimal riskFactor = 1.0M; // Example: simple risk adjustment
            decimal adjustedPremium = basePremium * riskFactor;

            var calculation = new PremiumCalculation
            {
                PolicyId = policyId,
                CustomerId = customerId,
                BasePremium = basePremium,
                AdjustedPremium = adjustedPremium
            };

            _context.PremiumCalculations.Add(calculation);
            await _context.SaveChangesAsync();
            return calculation;
        }
    }
}
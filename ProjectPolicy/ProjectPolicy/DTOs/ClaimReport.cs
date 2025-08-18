namespace InsurancePolicyMS.DTOs
{
    public class ClaimReport
    {
        public int PolicyId { get; set; }

        public int ClaimId { get; set; }

        public string PolicyType { get; set; }

        public decimal CoverageAmount { get; set; }

        public decimal ClaimAmount { get; set; }

    }
}

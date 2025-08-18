namespace InsurancePolicyMS.DTOs
{
    public class PolicyDto
    {
        public string PolicyType { get; set; }
        public decimal CoverageAmount { get; set; }
        public decimal PremiumAmount { get; set; }
        public DateTime ValidityStartDate { get; set; }
        public DateTime ValidityEndDate { get; set; }
    }
}

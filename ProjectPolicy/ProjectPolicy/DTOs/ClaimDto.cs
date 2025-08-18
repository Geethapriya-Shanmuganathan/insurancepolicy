using InsurancePolicyMS.Models;
using System.ComponentModel.DataAnnotations;
 
namespace InsurancePolicyMS.DTOs
{
    public class ClaimDto
    {
        public int PolicyId { get; set; }
        public int ClaimId { get; set; }
        public decimal ClaimAmount { get; set; }
        public ClaimStatus ClaimStatus { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? SettlementDate { get; set; }
        public string PolicyType { get; set; }
    }
}

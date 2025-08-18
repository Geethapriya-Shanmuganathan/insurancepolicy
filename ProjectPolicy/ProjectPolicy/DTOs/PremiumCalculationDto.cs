using System.ComponentModel.DataAnnotations;
 
namespace InsurancePolicyMS.DTOs
{
    public class PremiumCalculationDto
    {
        // PolicyId is required to identify the policy for calculation
        [Required(ErrorMessage = "PolicyId is required.")]
        public int PolicyId { get; set; }

        // CustomerId is required to identify the customer for calculation
        [Required(ErrorMessage = "CustomerId is required.")]
        public int CustomerId { get; set; }

        // You could add other relevant calculation parameters here if needed,
        // such as effective date, specific coverage options, etc.
        // For example:
        // [DataType(DataType.Date)]
        // public DateTime EffectiveDate { get; set; }
    }
}

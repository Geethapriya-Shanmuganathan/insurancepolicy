using InsurancePolicyMS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsurancePolicyMS.Models
{
    public class PremiumCalculation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CalculationId { get; set; }

        [ForeignKey("Policy")]
        public int PolicyId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal BasePremium { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal AdjustedPremium { get; set; }

        public Policy Policy { get; set; }

        public Customer Customer { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InsurancePolicyMS.Models;

namespace InsurancePolicyMS.Models
{
    public class Policy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PolicyId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PolicyType { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CoverageAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PremiumAmount { get; set; }

        [DataType(DataType.Date)]
        public DateTime ValidityStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ValidityEndDate { get; set; }

        public ICollection<Claim> Claims { get; set; }
        public ICollection<PremiumCalculation> PremiumCalculations { get; set; }
    }
}
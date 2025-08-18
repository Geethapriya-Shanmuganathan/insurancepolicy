using InsurancePolicyMS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InsurancePolicyMS.Models
{
    public class Claim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClaimId { get; set; }

        [ForeignKey("Policy")]
        public int PolicyId { get; set; }

        public Policy Policy { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ClaimAmount { get; set; }

        [Required]
        [EnumDataType(typeof(ClaimStatus))]
        public ClaimStatus ClaimStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime SubmissionDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SettlementDate { get; set; }
    }

    public enum ClaimStatus
    {
        PENDING,
        APPROVED,
        REJECTED
    }
}

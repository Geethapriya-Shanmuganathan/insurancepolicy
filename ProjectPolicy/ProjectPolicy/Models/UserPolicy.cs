//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace InsurancePolicyMS.Models
//{
//    public class UserPolicy
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int UserPolicyId { get; set; }

//        [ForeignKey("User")] public int UserId { get; set; }
//        [ForeignKey("Policy")] public int PolicyId { get; set; }

//        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

//        public User User { get; set; }
//        public Policy Policy { get; set; }
//    }
//}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsurancePolicyMS.Models
{
    public class UserPolicy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserPolicyId { get; set; }

        [ForeignKey("User")] public int UserId { get; set; }
        [ForeignKey("Policy")] public int PolicyId { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public Policy Policy { get; set; }
    }
}
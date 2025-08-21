using System.Collections.Generic;

namespace InsurancePolicyMS.Models
{
    public class UserProfilePage
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public List<Policy> PurchasedPolicies { get; set; } = new();
        public List<Claim> Claims { get; set; } = new();
    }
}
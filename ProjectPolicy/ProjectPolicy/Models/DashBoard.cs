using System.Collections.Generic;
namespace InsurancePolicyMS.Models
{
    public class DashBoard
    {
        public string Username { get; set; } = string.Empty;
        public List<Policy> PurchasedPolicies { get; set; } = new();
        public List<Policy> SuggestedPolicies { get; set; } = new();
        public bool HasPurchased => PurchasedPolicies != null && PurchasedPolicies.Count > 0;
    }
}



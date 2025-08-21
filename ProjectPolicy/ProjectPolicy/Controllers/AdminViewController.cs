using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePolicyMS.Controllers
{
    
    [Authorize(Roles = "ADMIN")]
    public class AdminViewController : Controller
    {
        public IActionResult Index() => View();          // /AdminView/Index
        public IActionResult Policies() => View();       // /AdminView/Policies
        public IActionResult Claims() => View();         // /AdminView/Claims
        public IActionResult UserPolicies() => View();   // /AdminView/UserPolicies
        public IActionResult Reports() => View();        // /AdminView/Reports
    }
}
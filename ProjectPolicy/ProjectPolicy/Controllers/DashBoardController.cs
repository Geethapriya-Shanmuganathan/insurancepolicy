//using InsurancePolicyMS.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace InsurancePolicyMS.Controllers
//{
//    public class DashBoardController : Controller
//    {
//        private readonly InsuranceDbContext _db;

//        public DashBoardController(InsuranceDbContext db)
//        {
//            _db = db;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var usernameClaim = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "username")?.Value;
//            var cookieUsername = Request.Cookies["ipms_username"];
//            var queryUsername = HttpContext.Request.Query["u"].FirstOrDefault();
//            var username = usernameClaim ?? cookieUsername ?? queryUsername ?? "Guest";

//            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
//            var allPolicies = await _db.Policies.AsNoTracking().ToListAsync();

//            var purchased = new List<Policy>();
//            if (user != null)
//            {
//                var purchasedPolicyIds = await _db.UserPolicies
//                    .Where(up => up.UserId == user.UserId)
//                    .Select(up => up.PolicyId)
//                    .ToListAsync();

//                purchased = allPolicies
//                    .Where(p => purchasedPolicyIds.Contains(p.PolicyId))
//                    .ToList();

//                var suggested = allPolicies
//                    .Where(p => !purchasedPolicyIds.Contains(p.PolicyId))
//                    .ToList();

//                ViewBag.HideAuthLinks = true;
//                ViewBag.Username = username;

//                var vm = new DashBoard
//                {
//                    Username = username,
//                    PurchasedPolicies = purchased,
//                    SuggestedPolicies = suggested
//                };

//                return View(vm);
//            }

//            // If no user found, show everything under Explore
//            ViewBag.HideAuthLinks = true;
//            ViewBag.Username = username;
//            var vmGuest = new DashBoard
//            {
//                Username = username,
//                PurchasedPolicies = new List<Policy>(),
//                SuggestedPolicies = allPolicies
//            };
//            return View(vmGuest);
//        }

//    }
//}



using InsurancePolicyMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyMS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly InsuranceDbContext _db;

        public DashboardController(InsuranceDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var usernameClaim = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "username")?.Value;
            var cookieUsername = Request.Cookies["ipms_username"];
            var queryUsername = HttpContext.Request.Query["u"].FirstOrDefault();
            var username = usernameClaim ?? cookieUsername ?? queryUsername ?? "Guest";

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
            var allPolicies = await _db.Policies.AsNoTracking().ToListAsync();

            var purchased = new List<Policy>();
            var suggested = allPolicies;

            if (user != null)
            {
                var purchasedPolicyIds = await _db.UserPolicies
                    .Where(up => up.UserId == user.UserId)
                    .Select(up => up.PolicyId)
                    .ToListAsync();

                purchased = allPolicies.Where(p => purchasedPolicyIds.Contains(p.PolicyId)).ToList();
                suggested = allPolicies.Where(p => !purchasedPolicyIds.Contains(p.PolicyId)).ToList();
            }

            ViewBag.HideAuthLinks = true;
            ViewBag.Username = username;

            var vm = new DashBoard
            {
                Username = username,
                PurchasedPolicies = purchased,
                SuggestedPolicies = suggested
            };

            return View(vm);
        }
    }
}
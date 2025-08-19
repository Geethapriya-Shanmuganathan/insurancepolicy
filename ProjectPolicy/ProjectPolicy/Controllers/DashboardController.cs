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
            // In a real app, extract username from auth context/JWT
            var username = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "username")?.Value
                           ?? HttpContext.Request.Query["u"].FirstOrDefault()
                           ?? "Guest";

            // Demo: purchased policies not modeled via a join table; simulate by none
            var allPolicies = await _db.Policies.AsNoTracking().ToListAsync();
            var purchased = new List<Policy>();
            var suggested = allPolicies;

            ViewBag.HideAuthLinks = true; // hide Login/Register in header

            var vm = new DashboardViewModel
            {
                Username = username,
                PurchasedPolicies = purchased,
                SuggestedPolicies = suggested
            };

            return View(vm);
        }
    }
}


using InsurancePolicyMS.Models;
using InsurancePolicyMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyMS.Controllers
{
    [Authorize]
    public class UserPremiumController : Controller
    {
        private readonly InsuranceDbContext _db;
        private readonly IPremiumCalculationService _premiumService;

        public UserPremiumController(InsuranceDbContext db, IPremiumCalculationService premiumService)
        {
            _db = db; _premiumService = premiumService;
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            var username = User?.Claims?.FirstOrDefault(c => c.Type == "username")?.Value
                           ?? Request.Cookies["ipms_username"];
            if (string.IsNullOrWhiteSpace(username)) return null;
            return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync(); if (user == null) return Unauthorized();
            ViewBag.Username = user.Username; ViewBag.HideAuthLinks = true;
            var policies = await _db.Policies.AsNoTracking().ToListAsync();
            return View(policies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(int policyId)
        {
            var user = await GetCurrentUserAsync(); if (user == null) return Unauthorized();
            var calc = await _premiumService.CalculatePremium(policyId, 0);
            return Json(new { basePremium = calc.BasePremium, adjustedPremium = calc.AdjustedPremium });
        }
    }
}

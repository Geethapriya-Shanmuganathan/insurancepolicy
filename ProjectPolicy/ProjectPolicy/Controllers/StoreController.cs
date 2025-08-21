using InsurancePolicyMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyMS.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private readonly InsuranceDbContext _db;

        public StoreController(InsuranceDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        private async Task<User?> GetCurrentUserAsync()
        {
            var username = User?.Claims?.FirstOrDefault(c => c.Type == "username")?.Value
                           ?? Request.Cookies["ipms_username"];
            if (string.IsNullOrWhiteSpace(username)) return null;
            return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        [HttpGet]
        public async Task<IActionResult> Explore()
        {
            var user = await GetCurrentUserAsync();
            ViewBag.Username = user?.Username ?? "Guest";
            ViewBag.HideAuthLinks = true;

            var policies = await _db.Policies.AsNoTracking().ToListAsync();
            var purchasedIds = new HashSet<int>();
            if (user != null)
            {
                purchasedIds = new HashSet<int>(await _db.UserPolicies
                    .Where(up => up.UserId == user.UserId)
                    .Select(up => up.PolicyId)
                    .ToListAsync());
            }

            ViewBag.PurchasedIds = purchasedIds;
            return View(policies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int policyId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized();
            var exists = await _db.UserPolicies.AnyAsync(up => up.UserId == user.UserId && up.PolicyId == policyId);
            if (exists) return RedirectToAction("Explore");
            var policyExists = await _db.Policies.AnyAsync(p => p.PolicyId == policyId);
            if (!policyExists) return NotFound();
            _db.UserPolicies.Add(new UserPolicy { UserId = user.UserId, PolicyId = policyId, PurchaseDate = DateTime.UtcNow });
            await _db.SaveChangesAsync();
            return RedirectToAction("Explore");
        }

        public async Task<IActionResult> MyPolicies()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized();
            ViewBag.Username = user.Username;
            ViewBag.HideAuthLinks = true;
            var list = await _db.UserPolicies
                .Include(up => up.Policy)
                .Where(up => up.UserId == user.UserId)
                .ToListAsync();
            return View(list);
        }
    }
}

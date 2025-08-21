using InsurancePolicyMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyMS.Controllers
{
    [Authorize]
    public class UserClaimsController : Controller
    {
        private readonly InsuranceDbContext _db;

        public UserClaimsController(InsuranceDbContext db)
        {
            _db = db;
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
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized();
            ViewBag.Username = user.Username; ViewBag.HideAuthLinks = true;

            var policyIds = await _db.UserPolicies.Where(up => up.UserId == user.UserId).Select(up => up.PolicyId).ToListAsync();
            var claims = await _db.Claims.Include(c => c.Policy).Where(c => policyIds.Contains(c.PolicyId)).ToListAsync();
            return View(claims);
        }

        [HttpGet]
        public async Task<IActionResult> Submit()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized();
            ViewBag.Username = user.Username; ViewBag.HideAuthLinks = true;
            var myPolicies = await _db.UserPolicies.Include(up => up.Policy).Where(up => up.UserId == user.UserId).ToListAsync();
            return View(myPolicies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int policyId, decimal claimAmount)
        {
            var user = await GetCurrentUserAsync(); if (user == null) return Unauthorized();
            var owns = await _db.UserPolicies.AnyAsync(up => up.UserId == user.UserId && up.PolicyId == policyId);
            if (!owns) return BadRequest("You do not own this policy.");
            var policy = await _db.Policies.FindAsync(policyId); if (policy == null) return NotFound();
            if (claimAmount <= 0 || claimAmount > policy.CoverageAmount) return BadRequest("Invalid claim amount.");
            var claim = new Claim { PolicyId = policyId, ClaimAmount = claimAmount, ClaimStatus = ClaimStatus.PENDING, SubmissionDate = DateTime.UtcNow };
            _db.Claims.Add(claim); await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

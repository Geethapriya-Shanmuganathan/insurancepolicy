using InsurancePolicyMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyMS.Controllers
{
    [Authorize]// both USER and ADMIN can view profile
    [Route("Profile")]
    public class UserProfileController : Controller
    {
        private readonly InsuranceDbContext _db;
        private readonly PasswordHasher<User> _hasher;

        public UserProfileController(InsuranceDbContext db)
        {
            _db = db;
            _hasher = new PasswordHasher<User>();
        }

        private string ResolveUsername()
        {
            var claim = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "username")?.Value;
            var cookie = Request.Cookies["ipms_username"];
            var query = HttpContext.Request.Query["u"].FirstOrDefault();
            return claim ?? cookie ?? query ?? "Guest";
        }

        public async Task<IActionResult> Index()
        {
            var username = ResolveUsername();
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
            var userId = user?.UserId ?? 0;

            var purchasedPolicyIds = userId == 0
                ? new List<int>()
                : await _db.UserPolicies.Where(up => up.UserId == userId).Select(up => up.PolicyId).ToListAsync();

            var purchased = purchasedPolicyIds.Count == 0
                ? new List<Policy>()
                : await _db.Policies.Where(p => purchasedPolicyIds.Contains(p.PolicyId)).ToListAsync();

            var claims = purchasedPolicyIds.Count == 0
                ? new List<Claim>()
                : await _db.Claims.Include(c => c.Policy).Where(c => purchasedPolicyIds.Contains(c.PolicyId)).ToListAsync();

            ViewBag.Username = username;      // used in dashboard layout
            ViewBag.HideAuthLinks = true;     // hide login/register in shared layout if used somewhere

            var vm = new UserProfilePage
            {
                Username = username,
                Role = (user?.Role.ToString() ?? "USER"),
                PurchasedPolicies = purchased,
                Claims = claims
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var username = ResolveUsername();
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return Unauthorized();

            var result = _hasher.VerifyHashedPassword(user, user.Password, currentPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest(new { message = "Current password is incorrect." });
            }

            user.Password = _hasher.HashPassword(user, newPassword);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Password updated successfully." });
        }
    }
}

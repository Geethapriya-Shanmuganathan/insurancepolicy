using InsurancePolicyMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class AdminController : ControllerBase
    {
        private readonly InsuranceDbContext _db;

        public AdminController(InsuranceDbContext db)
        {
            _db = db;
        }

        // Policies CRUD
        [HttpGet("policies")] public async Task<IActionResult> GetPolicies() => Ok(await _db.Policies.ToListAsync());
        [HttpPost("policies")] public async Task<IActionResult> CreatePolicy([FromBody] Policy p) { _db.Policies.Add(p); await _db.SaveChangesAsync(); return Ok(p); }
        [HttpPut("policies/{id}")] public async Task<IActionResult> UpdatePolicy(int id, [FromBody] Policy p) { var e = await _db.Policies.FindAsync(id); if (e==null) return NotFound(); e.PolicyType=p.PolicyType; e.CoverageAmount=p.CoverageAmount; e.PremiumAmount=p.PremiumAmount; e.ValidityStartDate=p.ValidityStartDate; e.ValidityEndDate=p.ValidityEndDate; await _db.SaveChangesAsync(); return Ok(e); }
        [HttpDelete("policies/{id}")] public async Task<IActionResult> DeletePolicy(int id) { var e = await _db.Policies.FindAsync(id); if (e==null) return NotFound(); _db.Policies.Remove(e); await _db.SaveChangesAsync(); return NoContent(); }

        // Claims processing
        [HttpGet("claims")] public async Task<IActionResult> GetClaims() => Ok(await _db.Claims.Include(c=>c.Policy).ToListAsync());
        [HttpPut("claims/{id}/status")] public async Task<IActionResult> SetClaimStatus(int id, [FromQuery] ClaimStatus status) { var c = await _db.Claims.FindAsync(id); if (c==null) return NotFound(); c.ClaimStatus = status; c.SettlementDate = DateTime.UtcNow; await _db.SaveChangesAsync(); return Ok(c); }

        // User policies
        [HttpGet("user/{userId}/policies")] public async Task<IActionResult> GetUserPolicies(int userId)
        {
            var data = await _db.UserPolicies.Include(up=>up.Policy).Where(up=>up.UserId==userId).ToListAsync();
            return Ok(data);
        }
        [HttpPost("user/{userId}/policies/{policyId}")] public async Task<IActionResult> AssignPolicy(int userId, int policyId)
        {
            if (!await _db.Users.AnyAsync(u=>u.UserId==userId) || !await _db.Policies.AnyAsync(p=>p.PolicyId==policyId)) return NotFound();
            if (await _db.UserPolicies.AnyAsync(up=>up.UserId==userId && up.PolicyId==policyId)) return Conflict("Already assigned");
            _db.UserPolicies.Add(new UserPolicy{ UserId=userId, PolicyId=policyId, PurchaseDate=DateTime.UtcNow });
            await _db.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("user/{userId}/policies/{policyId}")] public async Task<IActionResult> RemoveUserPolicy(int userId, int policyId)
        {
            var up = await _db.UserPolicies.FirstOrDefaultAsync(x=>x.UserId==userId && x.PolicyId==policyId);
            if (up==null) return NotFound();
            _db.UserPolicies.Remove(up);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // Simple report: policies count and claims by status
        [HttpGet("reports/overview")] public async Task<IActionResult> Overview()
        {
            var totalPolicies = await _db.Policies.CountAsync();
            var totalUsers = await _db.Users.CountAsync();
            var claimsByStatus = await _db.Claims.GroupBy(c=>c.ClaimStatus).Select(g=> new { Status=g.Key, Count=g.Count()}).ToListAsync();
            return Ok(new { totalPolicies, totalUsers, claimsByStatus });
        }
    }
}


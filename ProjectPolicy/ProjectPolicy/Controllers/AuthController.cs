using InsurancePolicyMS.Models;
using InsurancePolicyMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePolicyMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new User
            {
                Username = dto.Username,
                Role = dto.Role
            };

            try
            {
                var registeredUser = await _authService.RegisterAsync(user, dto.Password);
                return Ok(new { message = "User registered", user = registeredUser });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var token = await _authService.LoginAsync(dto.Username, dto.Password);
            if (token == null) return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { token });
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logout successful." });
        }
    }
}

using InsurancePolicyMS.Models;
using InsurancePolicyMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePolicyMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }



        [HttpPost("register")]

        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto registrationDto)

        {

            // Validate the incoming DTO based on its data annotations

            if (!ModelState.IsValid)

            {

                return BadRequest(ModelState); // Returns validation errors to the client

            }

            // Create a new User object from the DTO.

            // Note: The raw password is NOT stored directly in the User model here.

            // It is passed to the service, which should handle hashing it.

            var newUser = new User

            {

                Username = registrationDto.Username,

                Role = registrationDto.Role,

                // Password property of the User model will be set by the service after hashing

                // You should NOT set newUser.Password = registrationDto.Password directly here.

            };

            try

            {

                // Call your service to register the user, passing the raw password for hashing

                var registeredUser = await _userService.RegisterUser(newUser, registrationDto.Password);

                // Return a 201 Created status with the location of the new resource

                // and the registered user object (without raw password) in the response body.

                return CreatedAtAction(nameof(GetUserProfile), new { userId = registeredUser.UserId }, registeredUser);

            }

            catch (InvalidOperationException ex)

            {

                // Handle specific business logic errors, e.g., username already exists

                return BadRequest(new { message = ex.Message });

            }

            catch (Exception ex)

            {

                // Catch any other unexpected errors

                return StatusCode(500, "An error occurred during user registration: " + ex.Message);

            }

        }

        [HttpPost("login")]

        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto loginDto)

        {

            // Validate the incoming DTO based on its data annotations

            if (!ModelState.IsValid)

            {

                return BadRequest(ModelState); // Returns validation errors to the client

            }

            // Call your service to authenticate the user

            var user = await _userService.LoginUser(loginDto.Username, loginDto.Password);

            if (user == null)

            {

                return Unauthorized(new { message = "Invalid username or password." });

            }
            return Ok(new { message = "Login successful.", user });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var user = await _userService.GetUserProfile(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}


using System.ComponentModel.DataAnnotations;

namespace InsurancePolicyMS.Models
{
    public class UserRegistrationDto
    {
        //[Required]//
        //public string Username { get; set; }
        //[Required]//
        //public string Password { get; set; }
        //[Required]//
        //public UserRole Role { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public UserRole Role { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace InsurancePolicyMS.DTOs
{
    public class CustomerDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        public string Address { get; set; }
    }
}

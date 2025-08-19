using InsurancePolicyMS.DTOs;
using InsurancePolicyMS.Models;
using InsurancePolicyMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePolicyMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        //[HttpPost]
        //public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        //{
        //    var newCustomer = await _customerService.AddCustomer(customer);
        //    return CreatedAtAction(nameof(GetCustomerDetails), new { customerId = newCustomer.CustomerId }, newCustomer);
        //}
        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDto customerDto)
        {
            // Map the DTO to the Customer model
            var customer = new Customer
            {
                Name = customerDto.Name,
                Email = customerDto.Email,
                Phone = customerDto.Phone,
                Address = customerDto.Address
            };

            var newCustomer = await _customerService.AddCustomer(customer);

            return CreatedAtAction(nameof(GetCustomerDetails), new { customerId = newCustomer.CustomerId }, newCustomer);
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(int customerId, [FromBody] Customer customer)
        {
            var updatedCustomer = await _customerService.UpdateCustomer(customerId, customer);
            if (updatedCustomer == null)
            {
                return NotFound();
            }
            return Ok(updatedCustomer);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerDetails(int customerId)
        {
            var customer = await _customerService.GetCustomerDetails(customerId);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
    }
}
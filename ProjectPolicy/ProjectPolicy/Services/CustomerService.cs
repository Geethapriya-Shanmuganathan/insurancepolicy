using InsurancePolicyMS.Models;

namespace InsurancePolicyMS.Services
{
    public class CustomerService: ICustomerService
    {
        private readonly InsuranceDbContext _context;

        public CustomerService(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateCustomer(int customerId, Customer updatedCustomer)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return null;
            }

            customer.Name = updatedCustomer.Name;
            customer.Email = updatedCustomer.Email;
            customer.Phone = updatedCustomer.Phone;
            customer.Address = updatedCustomer.Address;

            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> GetCustomerDetails(int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }
    }
}

using InsurancePolicyMS.Models;

namespace InsurancePolicyMS.Services
{
    public interface ICustomerService
    {
        Task<Customer> AddCustomer(Customer customer);
        Task<Customer> UpdateCustomer(int customerId, Customer customer);
        Task<Customer> GetCustomerDetails(int customerId);
    }
}

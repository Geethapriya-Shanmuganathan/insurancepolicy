using InsurancePolicyMS.Models;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyMS.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(User user, string password);
        Task<string> LoginAsync(string username, string password);
    }
}

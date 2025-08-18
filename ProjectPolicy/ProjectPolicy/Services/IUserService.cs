//using InsurancePolicyMS.Models;

//namespace InsurancePolicyMS.Services
//{
//    public interface IUserService
//    {
//        Task<User> RegisterUser(User user, string plainPassword);
//        Task<User> LoginUser(string username, string plainPassword);
//        Task<User> GetUserProfile(int userId);
//    }
//}
using InsurancePolicyMS.Models;

namespace InsurancePolicyMS.Services
{
    public interface IUserService
    {
        Task<User> RegisterUser(User user, string password);
        Task<User?> LoginUser(string username, string password);
        Task<User?> GetUserProfile(int userId);
    }
}

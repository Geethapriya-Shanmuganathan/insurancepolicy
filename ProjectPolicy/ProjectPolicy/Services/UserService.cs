//using InsurancePolicyMS.Models;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Cryptography;

//namespace InsurancePolicyMS.Services
//{
//    public class UserService : IUserService
//    {
//        private readonly InsuranceDbContext _context;

//        public UserService(InsuranceDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<User> RegisterUser(User user, string plainPassword)
//        {
//            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
//            {
//                throw new InvalidOperationException("Username already exists.");
//            }

//            // Hash the password using a cryptographically strong function.
//            // Using a simple placeholder here. In a real application, you would use
//            // a library like BCrypt.Net or ASP.NET Core Identity's PasswordHasher.
//            user.Password = HashPassword(plainPassword);
//            user.Role = UserRole.USER; // Default role

//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();
//            return user;
//        }

//        public async Task<User> LoginUser(string username, string plainPassword)
//        {
//            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
//            if (user == null)
//            {
//                return null;
//            }

//            // Verify the hashed password.
//            if (VerifyPassword(plainPassword, user.Password))
//            {
//                return user;
//            }

//            return null;
//        }

//        public async Task<User> GetUserProfile(int userId)
//        {
//            return await _context.Users.FindAsync(userId);
//        }

//        // Placeholder for password hashing. Do not use this in a production environment.
//        private string HashPassword(string password)
//        {
//            using (var sha256 = SHA256.Create())
//            {
//                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
//            }
//        }

//        // Placeholder for password verification. Do not use this in a production environment.
//        private bool VerifyPassword(string plainPassword, string hashedPassword)
//        {
//            string hashedInput = HashPassword(plainPassword);
//            return string.Equals(hashedInput, hashedPassword, StringComparison.OrdinalIgnoreCase);
//        }
//    }
//}
//using InsurancePolicyMS.Data;
using InsurancePolicyMS.Models;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyMS.Services
{
    public class UserService : IUserService
    {
        private readonly InsuranceDbContext _context;

        public UserService(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterUser(User user, string password)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already exists.");
            }

            user.Password = password; // In production, hash this!
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> LoginUser(string username, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<User?> GetUserProfile(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }
}

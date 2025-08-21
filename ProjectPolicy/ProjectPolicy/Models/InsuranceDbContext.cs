using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyMS.Models
{
    public class InsuranceDbContext : DbContext
    {
        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Policy> Policies { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PremiumCalculation> PremiumCalculations { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<UserPolicy> UserPolicies { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Claim>()
                .Property(c => c.ClaimStatus)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Policy>()
                .Property(p => p.CoverageAmount)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Policy>()
                .Property(p => p.PremiumAmount)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Claim>()
                .Property(c => c.ClaimAmount)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<PremiumCalculation>()
                .Property(p => p.BasePremium)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<PremiumCalculation>()
                .Property(p => p.AdjustedPremium)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<UserPolicy>()
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPolicy>()
                .HasOne(up => up.Policy)
                .WithMany()
                .HasForeignKey(up => up.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
// adding authentication and authorisation

//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore;
//using Microsoft.EntityFrameworkCore;
////using PolicyApi.Data; // Assuming ApplicationUser and Policy are here

//namespace InsurancePolicyMS.Models
//{
//    public class InsuranceDbContext :
//    {
//        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
//            : base(options)
//        {
//        }

//        public DbSet<Policy> Policies { get; set; }
//        public DbSet<Claim> Claims { get; set; }
//        public DbSet<Customer> Customers { get; set; }
//        public DbSet<PremiumCalculation> PremiumCalculations { get; set; }
//        public DbSet<User> Users { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder); // Important for Identity

//            modelBuilder.Entity<Claim>()
//                .Property(c => c.ClaimStatus)
//                .HasConversion<string>();

//            modelBuilder.Entity<User>()
//                .Property(u => u.Role)
//                .HasConversion<string>();

//            modelBuilder.Entity<Policy>()
//                .Property(p => p.CoverageAmount)
//                .HasColumnType("decimal(10,2)");

//            modelBuilder.Entity<Policy>()
//                .Property(p => p.PremiumAmount)
//                .HasColumnType("decimal(10,2)");

//            modelBuilder.Entity<Claim>()
//                .Property(c => c.ClaimAmount)
//                .HasColumnType("decimal(10,2)");

//            modelBuilder.Entity<PremiumCalculation>()
//                .Property(p => p.BasePremium)
//                .HasColumnType("decimal(10,2)");

//            modelBuilder.Entity<PremiumCalculation>()
//                .Property(p => p.AdjustedPremium)
//                .HasColumnType("decimal(10,2)");
//        }
//    }
//}

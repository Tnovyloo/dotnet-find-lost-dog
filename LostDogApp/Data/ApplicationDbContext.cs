using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LostDogApp.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;


namespace LostDogApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDataProtectionKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LostDogReport> LostDogReports { get; set; }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set up relationship between LostDogReport and ApplicationUser
            modelBuilder.Entity<LostDogReport>()
                .HasOne<ApplicationUser>() // No navigation property yet
                .WithMany()                // Or .WithMany(u => u.LostDogReports) if you add collection to ApplicationUser
                .HasForeignKey(r => r.UserId) // Must match your LostDogReport property
                .OnDelete(DeleteBehavior.Cascade); // Optional: choose appropriate behavior
        }
    }

}

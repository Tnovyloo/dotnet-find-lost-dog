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
        public DbSet<City> Cities { get; set; }
        public DbSet<LostDogReportComment> LostDogReportComments { get; set; }


        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LostDogReport>()
                    .HasOne(r => r.User)
                    .WithMany(u => u.LostDogReports)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LostDogReport>()
                .HasOne(r => r.City)
                .WithMany(c => c.LostDogReports)
                .HasForeignKey(r => r.CityId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<LostDogReportComment>()
                .HasOne(c => c.LostDogReport)
                .WithMany(r => r.Comments)
                .HasForeignKey(c => c.LostDogReportId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LostDogReportComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.LostDogReportComments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

}

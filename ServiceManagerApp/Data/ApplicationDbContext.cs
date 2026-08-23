using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using ServiceManagerApp.Models.Entities;

namespace ServiceManagerApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<ServiceRequest> ServiceRequests { get; set; } = default!;
        public DbSet<Service> Services { get; set; } = default!;
        public DbSet<Worker> Workers { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Service>()
                .HasOne(s => s.ServiceRequest)
                .WithMany()
                .HasForeignKey(s => s.ServiceRequestId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

}

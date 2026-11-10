using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using ServiceManagerApp.Models.Entities;

namespace ServiceManagerApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<ServiceRequest> ServiceRequests { get; set; } = default!;
        public DbSet<Service> Services { get; set; } = default!;
        public DbSet<Worker> Workers { get; set; } = default!;
        public DbSet<Department> Departments { get; set; } = default!;
        public DbSet<JobRole> JobRoles { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Service>()
                .HasOne(s => s.ServiceRequest)
                .WithMany()
                .HasForeignKey(s => s.ServiceRequestId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<JobRole>()
                .HasOne(jr => jr.Department)
                .WithMany(d => d.JobRoles)
                .HasForeignKey(jr => jr.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Worker>()
                .HasOne(w => w.ApplicationUser)
                .WithOne()
                .HasForeignKey<Worker>(w => w.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

}

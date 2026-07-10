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
    }
}

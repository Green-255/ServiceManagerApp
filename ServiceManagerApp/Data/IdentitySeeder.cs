using Microsoft.AspNetCore.Identity;
using ServiceManagerApp.Authorization;

namespace ServiceManagerApp.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = [
                AppRoles.Administrator,
                AppRoles.Worker,
                AppRoles.Customer
            ];

            foreach (var role in roles) {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Failed to create role: {role}");
                    }
                }
            }
        }
    }
}

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ZTP_Project.Data
{
    /// <summary>
    /// Handles initial data seeding for roles and an admin user.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Initializes roles and an admin user in the database.
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving dependencies.</param>
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Admin", "Importer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string adminEmail = "test@example.com";
            string adminPassword = "StrongPassword123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating user: {error.Code} - {error.Description}");
                    }
                    throw new Exception($"Failed to create user {adminEmail}");
                }
            }

            var claims = await userManager.GetClaimsAsync(adminUser);
            if (!claims.Any(c => c.Type == "Permission" && c.Value == "Admin"))
            {
                await userManager.AddClaimAsync(adminUser, new Claim("Permission", "Admin"));
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
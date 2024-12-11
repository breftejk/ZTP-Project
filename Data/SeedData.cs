using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZTP_Project.Models;

namespace ZTP_Project.Data
{
    /// <summary>
    /// Handles initial data seeding for roles, an admin user, and base languages with words.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Initializes roles, an admin user, and base languages with words in the database.
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving dependencies.</param>
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            string[] roles = { "Admin", "Importer" };

            // Create roles
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create admin user
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

            // Seed languages
            if (!await dbContext.Languages.AnyAsync())
            {
                var languages = new[]
                {
                    new Language { Code = "de", Name = "German" },
                    new Language { Code = "fr", Name = "French" },
                    new Language { Code = "es", Name = "Spanish" },
                    new Language { Code = "it", Name = "Italian" }
                };

                dbContext.Languages.AddRange(languages);
                await dbContext.SaveChangesAsync();
            }

            // Seed words from JSON files
            var jsonFiles = new[] { "wwwroot/german.json", "wwwroot/french.json", "wwwroot/spanish.json", "wwwroot/italian.json" };

            foreach (var file in jsonFiles)
            {
                if (!File.Exists(file))
                {
                    Console.WriteLine($"File not found: {file}");
                    continue;
                }

                var jsonData = await File.ReadAllTextAsync(file);
                var wordPairs = JsonSerializer.Deserialize<List<List<string>>>(jsonData);

                if (wordPairs == null || wordPairs.Count <= 1)
                {
                    Console.WriteLine($"Invalid data in file: {file}");
                    continue;
                }

                var languageName = wordPairs[0][0]; // First row, first column is the language name
                var language = await dbContext.Languages.FirstOrDefaultAsync(l => l.Name == languageName);
                if (language == null)
                {
                    Console.WriteLine($"Language not found for file: {file}");
                    continue;
                }

                for (int i = 1; i < wordPairs.Count; i++) // Skip the first record
                {
                    var original = wordPairs[i][0];
                    var translation = wordPairs[i][1];

                    var word = new Word
                    {
                        Original = original,
                        Translation = translation,
                        LanguageId = language.Id
                    };

                    dbContext.Words.Add(word);
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
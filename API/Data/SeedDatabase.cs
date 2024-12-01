using System.Text.Json;
using System.IO;
using System.Linq;
using API.Models;

namespace API.Data;

/// <summary>
/// A utility class for seeding the database with word pairs from JSON files.
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Seeds the database with word pairs from JSON files located in the Data directory.
    /// </summary>
    /// <param name="context">The database context to use for seeding.</param>
    public static void SeedDatabase(AppDbContext context)
    {
        // List of supported languages with their names and codes
        var languages = new List<(string Code, string Name)>
        {
            ("de", "German"),
            ("fr", "French"),
            ("es", "Spanish"),
            ("it", "Italian")
        };

        foreach (var (code, name) in languages)
        {
            // Ensure the language exists in the database
            var language = context.Languages.FirstOrDefault(l => l.Code == code);
            if (language == null)
            {
                language = new Language { Code = code, Name = name };
                context.Languages.Add(language);
            }

            var filePath = Path.Combine("Data", $"{code}.json");

            // Check if the JSON file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} not found, skipping.");
                continue;
            }

            // Read the JSON file and parse it into a list of word pairs
            var jsonData = File.ReadAllText(filePath);
            var wordPairs = JsonSerializer.Deserialize<List<List<string>>>(jsonData);

            if (wordPairs == null)
            {
                Console.WriteLine($"Failed to parse {filePath}, skipping.");
                continue;
            }

            foreach (var pair in wordPairs)
            {
                // Ensure each word pair has exactly two elements (word and translation)
                if (pair.Count != 2) continue;

                var word = pair[0];
                var translation = pair[1];

                // Check if the word pair already exists
                if (!context.WordPairs
                    .Any(w => w.Word == word && 
                              w.Translation == translation && 
                              w.LanguageCode == code))
                {
                    context.WordPairs.Add(new WordPair
                    {
                        Word = word,
                        Translation = translation,
                        LanguageCode = code // Use LanguageCode instead of Language object
                    });
                }
            }
        }

        // Save changes to the database
        context.SaveChanges();
    }
}
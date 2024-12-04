using API.Models;
using API.Services;
using API.Strategies.DataImport;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
    private readonly IWordService _wordService;

    public ImportController(IWordService wordService)
    {
        _wordService = wordService ?? throw new ArgumentNullException(nameof(wordService));
    }

    /// <summary>
    /// Imports word pairs from different formats (CSV, JSON, XML).
    /// </summary>
    /// <returns>A list of imported word pairs or an error message.</returns>
    [HttpPost("words")]
    [Consumes("text/csv", "application/json", "application/xml")]
    public async Task<IActionResult> ImportWords()
    {
        try
        {
            // Read the request body asynchronously
            using var reader = new StreamReader(Request.Body);
            var data = await reader.ReadToEndAsync();

            Console.WriteLine(data); // Log for debugging

            string format = Request.ContentType?.ToLower() ?? string.Empty;
            List<WordPair> importedData;

            // Use a strategy pattern for different formats
            var importer = new DataImporter();

            switch (format)
            {
                case "application/json":
                    importer.SetImportStrategy(new JsonImportStrategy(_wordService));
                    break;

                case "text/csv":
                    importer.SetImportStrategy(new CsvImportStrategy(_wordService));
                    break;

                case "application/xml":
                    importer.SetImportStrategy(new XmlImportStrategy(_wordService));
                    break;

                default:
                    return BadRequest($"Unsupported format '{format}'. Supported formats are: json, csv, xml.");
            }

            // Import data using the selected strategy
            importedData = importer.ImportData(data);

            // Check if no valid data was imported
            if (importedData.Count == 0)
            {
                return BadRequest("No valid word pairs were imported.");
            }

            return Ok(importedData);
        }
        catch (Exception ex)
        {
            // Handle unexpected errors gracefully
            return BadRequest($"Error importing data: {ex.Message}");
        }
    }
}
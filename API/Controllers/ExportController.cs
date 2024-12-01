using API.Models;
using Microsoft.AspNetCore.Mvc;
using API.Services.DataExport;
using API.Services.Word;

namespace API.Controllers;

/// <summary>
/// API Controller for exporting word pairs in different formats.
/// </summary>
[ApiController]
[Route("api/export")]
public class ExportController : ControllerBase
{
    private readonly WordFacade _wordFacade;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportController"/> class.
    /// </summary>
    /// <param name="wordFacade">The facade used for accessing word-related operations.</param>
    public ExportController(WordFacade wordFacade)
    {
        _wordFacade = wordFacade;
    }

    /// <summary>
    /// Exports word pairs for a specific language or all languages in the specified format.
    /// </summary>
    /// <param name="languageCode">The language code to export word pairs for. Use "all" to export all languages.</param>
    /// <param name="format">
    /// The export format. Supported values:
    /// <list type="bullet">
    /// <item><term>json</term><description>Exports in JSON format.</description></item>
    /// <item><term>csv</term><description>Exports in CSV format.</description></item>
    /// <item><term>xml</term><description>Exports in XML format.</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// Returns the exported data in the specified format.
    /// </returns>
    /// <response code="200">Returns the exported data.</response>
    /// <response code="400">If the format is unsupported or the language is invalid.</response>
    /// <response code="404">If no word pairs are found for the specified language.</response>
    [HttpGet("words/{languageCode}/{format}")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public IActionResult ExportWords(string languageCode, string format)
    {
        try
        {
            List<WordPair> data;

            // Handle "all" to export all word pairs
            if (languageCode == "all")
            {
                data = _wordFacade.GetAllWords(null);
            }
            else
            {
                data = _wordFacade.GetAllWords(languageCode);
            }

            if (data.Count == 0)
            {
                return NotFound($"No word pairs found for language '{languageCode}'.");
            }

            // Use Strategy Pattern to select the export format
            var exporter = new DataExporter();

            switch (format.ToLower())
            {
                case "json":
                    exporter.SetExportStrategy(new JsonExportStrategy());
                    break;
                case "csv":
                    exporter.SetExportStrategy(new CsvExportStrategy());
                    break;
                case "xml":
                    exporter.SetExportStrategy(new XmlExportStrategy());
                    break;
                default:
                    return BadRequest($"Unsupported format '{format}'. Supported formats are: json, csv, xml.");
            }

            // Generate the export result
            var result = exporter.ExportData(data);
            return Ok(result);
        }
        catch (NotSupportedException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

/// <summary>
/// Supported export formats.
/// </summary>
public enum ExportFormat
{
    Json,
    Csv,
    Xml
}
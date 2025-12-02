using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CtrlAltEliteProject.Services;
using CtrlAltEliteProject.Services.Logging;

namespace CtrlAltEliteProject.Pages.Pdf
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly IPdfTextService _pdfTextService;
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserQueryLogger _queryLogger;

        public IndexModel(IWebHostEnvironment env, IPdfTextService pdfTextService, ILogger<IndexModel> logger, IUserQueryLogger queryLogger)
        {
            _env = env;
            _pdfTextService = pdfTextService;
            _logger = logger;
            _queryLogger = queryLogger;
        }

        [BindProperty]
        public IFormFile? PdfFile { get; set; }

        public IReadOnlyList<string>? ExtractedPages { get; private set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Ensure file exists
            if (PdfFile == null || PdfFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please upload a PDF.");
                return Page();
            }

            // Temp storage for reading PDF
            var tempFile = Path.Combine(Path.GetTempPath(),
                $"{Guid.NewGuid()}{Path.GetExtension(PdfFile.FileName)}");

            try
            {
                // Save uploaded file to temp path
                await using (var fs = System.IO.File.Create(tempFile))
                {
                    await PdfFile.CopyToAsync(fs);
                }

                // Extract text pages
                ExtractedPages = _pdfTextService.ExtractTextFromPages(tempFile)
                                   ?? Array.Empty<string>();

                // Save output to /Output folder
                var outputFolder = Path.Combine(_env.ContentRootPath, "Output");
                Directory.CreateDirectory(outputFolder);

                var originalName = Path.GetFileNameWithoutExtension(PdfFile.FileName) ?? "pdf";
                var safeName = string.Concat(originalName.Split(Path.GetInvalidFileNameChars())).Trim();
                if (string.IsNullOrWhiteSpace(safeName)) safeName = "pdf";

                var outFile = $"{safeName}_{DateTime.UtcNow:yyyyMMddHHmmss}.txt";
                var outPath = Path.Combine(outputFolder, outFile);

                var sb = new StringBuilder();

                for (int i = 0; i < ExtractedPages.Count; i++)
                {
                    sb.AppendLine($"--- Page {i + 1} ---");
                    sb.AppendLine(ExtractedPages[i] ?? "");
                    sb.AppendLine();
                }

                System.IO.File.WriteAllText(outPath, sb.ToString(), Encoding.UTF8);

                TempData["SavedFiles"] = outPath;

                // Log interaction as JSON
                try
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var combinedText = string.Join("\n\n---PAGE---\n\n", ExtractedPages ?? Array.Empty<string>());
                    var record = new InteractionRecord
                    {
                        UserId = userId,
                        Source = "pdf",
                        Input = new { fileName = PdfFile.FileName, textExtracted = combinedText },
                        Output = outPath,
                        Metadata = new Dictionary<string, object?> { ["pageCount"] = ExtractedPages?.Count ?? 0 }
                    };

                    var savedJsonPath = await _queryLogger.LogAsync(record);
                    if (!string.IsNullOrEmpty(savedJsonPath))
                    {
                        _logger.LogDebug("Saved interaction JSON: {path}", savedJsonPath);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to log PDF interaction; continuing.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PDF extraction failed.");
                ModelState.AddModelError(string.Empty,
                    "Failed to extract or save PDF: " + ex.Message);
            }
            finally
            {
                try { System.IO.File.Delete(tempFile); } catch { }
            }

            return Page();
        }
    }
}

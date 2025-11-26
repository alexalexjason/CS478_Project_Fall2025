using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TestingUglyToadPdfPig.Services;

namespace TestingUglyToadPdfPig.Pages.Voice
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly IPdfTextService _pdfTextService;

        public IndexModel(IWebHostEnvironment env, IPdfTextService pdfTextService)
        {
            _env = env;
            _pdfTextService = pdfTextService;
        }

        [BindProperty]
        public IFormFile? PdfFile { get; set; }

        [BindProperty]
        public string? VoiceText { get; set; }

        // Holds the extracted per-page text after processing
        public IReadOnlyList<string>? ExtractedPages { get; private set; }

        public void OnGet()
        {
        }

        // Default form POST: processes uploaded PDF + VoiceText together
        public async Task<IActionResult> OnPostAsync()
        {
            if (PdfFile == null && string.IsNullOrWhiteSpace(VoiceText))
            {
                ModelState.AddModelError(string.Empty, "Upload a PDF or provide voice text.");
                return Page();
            }

            if (PdfFile != null && PdfFile.Length > 0)
            {
                var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{Path.GetExtension(PdfFile.FileName)}");
                try
                {
                    await using (var fs = System.IO.File.Create(tempFile))
                    {
                        await PdfFile.CopyToAsync(fs);
                    }

                    // Use your existing service to extract text per page
                    ExtractedPages = _pdfTextService.ExtractTextFromPages(tempFile);
                }
                finally
                {
                    try { System.IO.File.Delete(tempFile); } catch { /* ignore */ }
                }
            }

            // VoiceText remains available in the bound property for display or further processing
            return Page();
        }

        // Keep existing Save handler (it still works if you want to save voice only)
        public IActionResult OnPostSave(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest();
            }

            try
            {
                var folder = Path.Combine(_env.ContentRootPath, "VoiceOutput");
                Directory.CreateDirectory(folder);

                var fileName = $"voice_{DateTime.UtcNow:yyyyMMddHHmmss}.txt";
                var path = Path.Combine(folder, fileName);

                System.IO.File.WriteAllText(path, text);

                return new JsonResult(new { success = true, savedTo = path });
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
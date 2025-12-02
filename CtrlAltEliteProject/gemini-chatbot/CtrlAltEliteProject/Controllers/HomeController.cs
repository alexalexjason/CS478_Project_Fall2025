using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CtrlAltEliteProject.Models;
using CtrlAltEliteProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CtrlAltEliteProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GeminiService _gemini;
        private readonly IPdfTextService _pdfService;

        public HomeController(ILogger<HomeController> logger, GeminiService gemini, IPdfTextService pdfService)
        {
            _logger = logger;
            _gemini = gemini;
            _pdfService = pdfService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Chatbot()
        {
            return View("Chatbot");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest req)
        {
            if (string.IsNullOrWhiteSpace(req?.Message))
                return Json(new { reply = "Please enter a message." });

            var reply = await _gemini.SendMessageAsync(req.Message);
            return Json(new { reply });
        }

        // New: POST /Home/UploadFile
        // Accepts a single file (form-data `file`) — currently expects PDFs and uses PdfTextService to extract text.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile()
        {
            var form = await Request.ReadFormAsync();
            var file = form.Files.FirstOrDefault();
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file uploaded." });

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".pdf")
                return BadRequest(new { error = "Only PDF files are supported." });

            // Save to temp file and extract text
            var tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".pdf");
            try
            {
                await using (var fs = System.IO.File.Create(tempPath))
                {
                    await file.CopyToAsync(fs);
                }

                var pages = _pdfService.ExtractTextFromPages(tempPath) ?? new string[0];
                var combined = string.Join("\n\n", pages);

                // Return a short snippet and full text (full may be large)
                var snippet = combined.Length > 1500 ? combined.Substring(0, 1500) + "…" : combined;
                return Json(new { fileName = file.FileName, textSnippet = snippet, fullText = combined });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error processing uploaded file");
                return StatusCode(500, new { error = "Failed to process uploaded file." });
            }
            finally
            {
                try { System.IO.File.Delete(tempPath); } catch { /* ignore */ }
            }
        }

        public class MessageRequest
        {
            public string? Message { get; set; }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

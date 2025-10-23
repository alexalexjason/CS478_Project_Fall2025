using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestingUglyToadPdfPig.Models;
using TestingUglyToadPdfPig.Services;

namespace TestingUglyToadPdfPig.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPdfTextService _pdfTextService;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IPdfTextService pdfTextService, IWebHostEnvironment env)
        {
            _logger = logger;
            _pdfTextService = pdfTextService;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                ModelState.AddModelError("pdfFile", "Please select a PDF file to upload.");
                return View();
            }

            var ext = Path.GetExtension(pdfFile.FileName);
            if (!string.Equals(ext, ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("pdfFile", "Only PDF files are supported.");
                return View();
            }

            // Create Output folder under app content root (project folder)
            var outputFolder = Path.Combine(_env.ContentRootPath, "Output");
            Directory.CreateDirectory(outputFolder);

            var safeName = Path.GetFileNameWithoutExtension(pdfFile.FileName);
            var savedPdfFileName = $"{safeName}_{Guid.NewGuid():N}{ext}";
            var savedPdfPath = Path.Combine(outputFolder, savedPdfFileName);

            await using (var stream = new FileStream(savedPdfPath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            IReadOnlyList<string> pages;
            try
            {
                // Use your existing service which accepts a file path
                pages = _pdfTextService.ExtractTextFromPages(savedPdfPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting text from uploaded PDF {File}", savedPdfFileName);
                ModelState.AddModelError("", "Failed to parse PDF. Check server logs for details.");
                return View();
            }

            var outputTxtFileName = Path.ChangeExtension(savedPdfFileName, ".txt");
            var outputTxtPath = Path.Combine(outputFolder, outputTxtFileName);

            var joined = string.Join(Environment.NewLine + Environment.NewLine, pages);
            await System.IO.File.WriteAllTextAsync(outputTxtPath, joined);

            ViewData["Message"] = "PDF parsed and saved to the Output folder.";
            ViewData["OutputFileName"] = outputTxtFileName;
            ViewData["OutputFilePath"] = outputTxtPath;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
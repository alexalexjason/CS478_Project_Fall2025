/*
 * Developer: Evan S.
 * Date Created: 10/19/2025
 * Last Updated: 10/19/2025
 * Description: For CS 478 Team Project
 *              This code is the controller for preprocessing
*/

using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using FilePreProcessingAPI.Services;

namespace FilePreProcessingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreprocessorController : ControllerBase
    {
        private readonly PdfProcessor _pdfProcessor;

        // Use dependency injection instead of new PdfProcessor()
        public PreprocessorController(PdfProcessor pdfProcessor)
        {
            _pdfProcessor = pdfProcessor;
        }

        /// <summary>
        /// Accepts a PDF file upload and returns extracted text.
        /// </summary>
        /// <param name="file">The uploaded PDF file.</param>
        /// <returns>Extracted text or error message.</returns>
        [HttpPost("extract-pdf-text")]
        public async Task<IActionResult> ExtractPdfText([FromForm] Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var tempFilePath = Path.GetTempFileName();

            try
            {
                // Save uploaded PDF temporarily
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Extract text from the saved PDF
                var extractedText = await _pdfProcessor.ExtractTextAsync(tempFilePath);

                return Ok(new { Text = extractedText });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            finally
            {
                // Clean up temp file
                if (System.IO.File.Exists(tempFilePath))
                {
                    System.IO.File.Delete(tempFilePath);
                }
            }
        }
    }
}

/*
 * Developer: Evan S.
 * Date Created: 10/19/2025
 * Last Updated: 10/23/2025
 * Description: For CS 478 Team Project
 *              This code is the controller for preprocessing
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using FilePreProcessingAPI.Services;
using System;

namespace FilePreProcessingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreprocessorController : ControllerBase
    {
        private readonly PdfProcessor _pdfProcessor;

        public PreprocessorController(PdfProcessor pdfProcessor)
        {
            _pdfProcessor = pdfProcessor;
        }

        [HttpPost("extract-pdf-text")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ExtractPdfText(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var tempFilePath = Path.GetTempFileName();

            try
            {
                // Fully qualify System.IO.File to avoid collision with ControllerBase.File
                await using (var fs = System.IO.File.Create(tempFilePath))
                {
                    await file.CopyToAsync(fs);
                }

                var extractedText = await _pdfProcessor.ExtractTextAsync(tempFilePath);

                return Ok(new { Text = extractedText });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            finally
            {
                if (System.IO.File.Exists(tempFilePath))
                    System.IO.File.Delete(tempFilePath);
            }
        }
    }
}
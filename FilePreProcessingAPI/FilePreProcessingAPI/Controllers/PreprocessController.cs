/*
 * Developer: Evan S.
 * Date Created: 10/19/2025
 * Last Updated: 10/19/2025
 * Description: For CS 478 Team Project
 *              This code is the controller for preprocessing
*/

using Microsoft.AspNetCore.Mvc;

namespace FilePreprocessingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreprocessController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public PreprocessController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string uploads = Path.Combine(_env.ContentRootPath, "Uploads");
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            string filePath = Path.Combine(uploads, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // --- Simple stub logic for now ---
            string extractedText;
            if (file.FileName.EndsWith(".pdf"))
            {
                extractedText = "[Pretend PDF text extracted here]";
            }
            else if (file.FileName.EndsWith(".mp3") || file.FileName.EndsWith(".wav"))
            {
                extractedText = "[Pretend audio transcribed text here]";
            }
            else
            {
                return BadRequest("Unsupported file type.");
            }

            string outputDir = Path.Combine(_env.ContentRootPath, "Outputs");
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            string txtFile = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(file.FileName) + ".txt");
            await System.IO.File.WriteAllTextAsync(txtFile, extractedText);

            return Ok(new
            {
                message = "File processed successfully",
                textFilePath = txtFile
            });
        }
    }
}

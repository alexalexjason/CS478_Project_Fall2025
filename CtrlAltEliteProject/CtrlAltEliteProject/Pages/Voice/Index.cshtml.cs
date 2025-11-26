using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;

namespace CtrlAltEliteProject.Pages.Voice
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public IndexModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnGet()
        {
        }

        // Handles POST from JS fetch form-data: ?handler=Save
        public IActionResult OnPostSave(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest();
            }

            try
            {
                // Save under the project content root in the Output folder
                var folder = Path.Combine(_env.ContentRootPath, "Output");
                Directory.CreateDirectory(folder);

                // Use a timestamped filename to avoid collisions
                var fileName = $"voice_{DateTime.UtcNow:yyyyMMddHHmmss}.txt";
                var path = Path.Combine(folder, fileName);

                System.IO.File.WriteAllText(path, text);

                // Return saved path for debugging; remove/sanitize in production
                return new JsonResult(new { success = true, savedTo = path });
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
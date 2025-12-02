using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using CtrlAltEliteProject.Services.Logging;

namespace CtrlAltEliteProject.Pages.Voice
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUserQueryLogger _queryLogger;

        public IndexModel(IWebHostEnvironment env, IUserQueryLogger queryLogger)
        {
            _env = env;
            _queryLogger = queryLogger;
        }

        public void OnGet()
        {
        }

        // Handles POST from JS fetch form-data: ?handler=Save
        public async Task<IActionResult> OnPostSave(string text)
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

                await System.IO.File.WriteAllTextAsync(path, text);

                // Attempt to log the interaction
                try
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var record = new InteractionRecord
                    {
                        UserId = userId,
                        Source = "audio",
                        Input = new { transcript = text },
                        Output = path
                    };

                    var savedJsonPath = await _queryLogger.LogAsync(record);
                }
                catch
                {
                    // Swallow to avoid breaking the response; logging is best-effort
                }

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
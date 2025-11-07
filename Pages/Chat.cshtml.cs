using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class ChatModel : PageModel
    {
        private readonly IHuggingFaceService _ai;

        public ChatModel(IHuggingFaceService ai)
        {
            _ai = ai;
        }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostSendAsync([FromForm] string message, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(message))
                return BadRequest(new { error = "Message is required." });

            var response = await _ai.GetResponseAsync(message, cancellationToken);
            return new JsonResult(new { bot = response });
        }
    }
}
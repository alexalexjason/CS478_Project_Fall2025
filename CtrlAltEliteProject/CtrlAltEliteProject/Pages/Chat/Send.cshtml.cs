using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CtrlAltEliteProject.Models;
using CtrlAltEliteProject.Services.Logging;

namespace CtrlAltEliteProject.Pages.Chat
{
    public class SendModel : PageModel
    {
        private readonly IUserQueryLogger _queryLogger;

        public SendModel(IUserQueryLogger queryLogger)
        {
            _queryLogger = queryLogger;
        }

        public async Task<IActionResult> OnPostAsync(string message, string? transcript)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest(new { error = "Message is required." });
            }

            var userId = Request.Cookies["UserId"] ?? Guid.NewGuid().ToString();
            if (!Request.Cookies.ContainsKey("UserId"))
            {
                Response.Cookies.Append("UserId", userId);
            }

            try
            {
                var record = new InteractionRecord
                {
                    UserId = userId,
                    Source = "chat",
                    Input = new { query = message, includesTranscript = !string.IsNullOrEmpty(transcript) },
                    Metadata = new Dictionary<string, object?>
                    {
                        ["transcriptLength"] = transcript?.Length ?? 0
                    }
                };

                await _queryLogger.LogAsync(record);

                var botReply = await SimulatedBotReplyAsync(message, transcript);

                return new JsonResult(new { success = true, reply = botReply });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        private async Task<string> SimulatedBotReplyAsync(string message, string? transcript)
        {
            return await Task.FromResult(
                transcript != null && transcript.Length > 0
                    ? $"Simulated bot reply to: '{message}' (with transcript context, {transcript.Length} chars)"
                    : $"Simulated bot reply to: '{message}'"
            );
        }
    }
}
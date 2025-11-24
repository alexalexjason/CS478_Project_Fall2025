using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        public record ChatRequest(string Message);
        public record ChatResponse(string Reply);

        // If you have a DbContext and want to persist messages, inject it here:
        // private readonly ApplicationDbContext _db;
        // public ChatController(ApplicationDbContext db) { _db = db; }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Message))
                return BadRequest();

            // Example: store message (uncomment and adapt if you have a DbContext)
            // var msg = new ChatMessage { Id = Guid.NewGuid(), ChatSessionId = /*session id*/, Sender = "user", Content = req.Message, CreatedUtc = DateTime.UtcNow };
            // _db.ChatMessages.Add(msg); await _db.SaveChangesAsync();

            // TODO: Replace with real chatbot integration (OpenAI, Azure OpenAI, Bot Framework, etc.)
            // For now, a simple echo reply:
            var reply = $"Echo: {req.Message}";
            var response = new ChatResponse(reply);

            // Optionally persist bot reply similarly

            return Ok(response);
        }
    }
}
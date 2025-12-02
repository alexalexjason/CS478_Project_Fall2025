using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CtrlAltEliteProject.Services.History;

namespace CtrlAltEliteProject.Pages.History
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUserQueryHistoryService _historyService;

        public IndexModel(IUserQueryHistoryService historyService)
        {
            _historyService = historyService;
        }

        public List<HistoryEntry>? Entries { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                Entries = await _historyService.GetUserHistoryAsync(userId);
            }
        }

        public async Task OnPostDeleteAsync(string interactionId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                await _historyService.DeleteHistoryEntryAsync(userId, interactionId);
            }

            await OnGetAsync();
        }
    }
}
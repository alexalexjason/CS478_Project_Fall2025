using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using CtrlAltEliteProject.Models;
using CtrlAltEliteProject.Services.Logging;

namespace CtrlAltEliteProject.Pages.ExtractedText
{
    public class IndexModel : PageModel
    {
        private readonly IUserQueryLogger _queryLogger;

        public List<InteractionRecord> Records { get; set; } = new();

        public IndexModel(IUserQueryLogger queryLogger)
        {
            _queryLogger = queryLogger;
        }

        public async Task OnGetAsync()
        {
            Records = await _queryLogger.GetAllRecordsAsync();
        }
    }
}

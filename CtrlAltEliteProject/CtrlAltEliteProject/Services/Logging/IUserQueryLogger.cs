using System.Collections.Generic;
using System.Threading.Tasks;
using CtrlAltEliteProject.Models;

namespace CtrlAltEliteProject.Services.Logging
{
    public interface IUserQueryLogger
    {
        Task LogAsync(InteractionRecord record);
        Task<List<InteractionRecord>> GetAllRecordsAsync();
    }
}
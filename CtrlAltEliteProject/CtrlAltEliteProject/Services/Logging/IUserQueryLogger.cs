using System.Threading.Tasks;

namespace CtrlAltEliteProject.Services.Logging
{
    public interface IUserQueryLogger
    {
        /// <summary>
        /// Logs an interaction record to a JSON file and returns the full saved path (or empty string on failure).
        /// </summary>
        Task<string> LogAsync(InteractionRecord record);
    }
}
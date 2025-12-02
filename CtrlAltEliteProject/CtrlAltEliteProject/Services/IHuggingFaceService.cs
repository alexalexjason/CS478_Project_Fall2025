using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public interface IHuggingFaceService
    {
        Task<string> GetResponseAsync(string input, CancellationToken cancellationToken = default);
    }
}
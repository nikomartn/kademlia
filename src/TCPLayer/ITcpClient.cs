using System.Threading;
using System.Threading.Tasks;

namespace TCPLayer
{
    public interface ITcpClient
    {
        Task<string> Communicate(string message, CancellationToken cancellationToken);
    }
}
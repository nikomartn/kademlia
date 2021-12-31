using System.Threading;
using System.Threading.Tasks;

namespace TCPLayer
{
    public interface ITcpServer
    {
        Task StartListeningAsync(CancellationToken cancellationToken);
    }
}
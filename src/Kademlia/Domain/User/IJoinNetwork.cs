using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Domain.User
{
    public interface IJoinNetwork
    {
        Task<bool> DoItAsync(string ip, string port, CancellationToken cancellationToken);
    }
}

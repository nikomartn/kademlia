using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Domain.User
{
    public interface IPublish
    {
        Task<string> DoIt(string data, CancellationToken cancellationToken);
    }
}

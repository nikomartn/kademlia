using Kademlia.Domain.Buckets.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Domain.Network
{
    public interface IFindNodeHandler
    {
        Task<Contact[]> OnReceivedFindNode(Contact sender, string key, CancellationToken cancellationToken);
    }
}

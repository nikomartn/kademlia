using Kademlia.Domain.Buckets.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Domain.Network
{
    public struct AknowledgePinged { }
    public interface IPingHandler
    {
        public Task<AknowledgePinged> OnReceivedPing(Contact by, CancellationToken cancellationToken);
    }
}

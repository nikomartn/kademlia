using Kademlia.Domain.Buckets.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Tuple = Kademlia.Domain.Database.Contracts.Tuple;

namespace Kademlia.Domain.Network
{
    public interface IStoreHandler
    {
        Task<bool> OnReceivedStore(Contact sender, Tuple tuple, CancellationToken cancellationToken);
    }
}

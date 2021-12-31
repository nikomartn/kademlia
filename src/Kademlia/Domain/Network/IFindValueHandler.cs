using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Models;
using System.Threading;
using System.Threading.Tasks;
namespace Kademlia.Domain.Network
{
    public interface IFindValueHandler
    {
        Task<FoundResult> OnReceivedFindValue(Contact sender, string key, CancellationToken cancellationToken);
    }
}

using BinaryStringLib;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Domain.Database.Contracts
{
    public interface IDatabase
    {
        Task<Tuple> FindValueForKeyAsync(BinaryString key, CancellationToken cancellationToken);
        Task<bool> StoreAsync(Tuple tuple, CancellationToken cancellationToken);
        Task Remove(BinaryString key, CancellationToken cancellationToken);
        Task<Tuple[]> CloneData();
    }
}

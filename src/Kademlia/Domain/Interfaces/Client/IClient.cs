using BinaryStringLib;
using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Database.Contracts;
using Kademlia.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Domain.Interfaces.Client
{
    public interface IClient
    {
        Task<bool> MakePing(Contact sender, Contact contact, CancellationToken cancellationToken);
        Task<Contact> RequestIdentification(string ip, int port, CancellationToken cancellationToken);
        Task<Contact[]> FindNodes(Contact sender, Contact contact, BinaryString id, CancellationToken cancellationToken);
        Task Store(Contact sender, Contact contact, Tuple tuple, CancellationToken cancellationToken);
        Task<FoundResult> FindValue(Contact sender, Contact contact, BinaryString id, CancellationToken cancellationToken);
    }
}

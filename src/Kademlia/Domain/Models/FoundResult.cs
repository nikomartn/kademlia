using Kademlia.Domain.Buckets.Contracts;
using Tuple = Kademlia.Domain.Database.Contracts.Tuple;
namespace Kademlia.Domain.Models
{
    public class FoundResult
    {
        public enum FoundType { FOUND, FOUND_NODES };
        public FoundType Type { get; set; }
        public Contact[] Contacts { get; set; }
        public Tuple Data { get; set; }
    }
}

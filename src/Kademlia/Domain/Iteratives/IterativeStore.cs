using Kademlia.Domain.Buckets;
using Kademlia.Domain.Interfaces.Client;
using System.Threading;
using System.Threading.Tasks;
using Tuple = Kademlia.Domain.Database.Contracts.Tuple;

namespace Kademlia.Domain.Iteratives
{
    public class IterativeStore
    {
        public IterativeStore(IterativeFindNode iterativeFindNode, IClient client, BucketContainer bucketContainer)
        {
            this.iterativeFindNode = iterativeFindNode;
            this.client = client;
            this.bucketContainer = bucketContainer;
        }

        private readonly IterativeFindNode iterativeFindNode;
        private readonly IClient client;
        private readonly BucketContainer bucketContainer;

        public async Task StoreAsync(Tuple tuple, CancellationToken cancellationToken)
        {
            var contacts = await iterativeFindNode.DoItAsync(tuple.Key, cancellationToken);
            foreach (var contact in contacts)
            {
                await client.Store(bucketContainer.Me, contact, tuple, cancellationToken);
            }
        }
    }
}

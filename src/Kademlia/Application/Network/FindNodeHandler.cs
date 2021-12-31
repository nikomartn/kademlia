using Kademlia.Domain.Buckets;
using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Network;
using Logger;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Application.Network
{
    public class FindNodeHandler : IFindNodeHandler
    {
        private readonly BucketContainer bucketContainer;
        private readonly ILogger<FindNodeHandler> logger;

        public FindNodeHandler(BucketContainer bucketContainer, ILogger<FindNodeHandler> logger)
        {
            this.bucketContainer = bucketContainer;
            this.logger = logger;
        }

        public async Task<Contact[]> OnReceivedFindNode(Contact sender, string key, CancellationToken cancellationToken)
        {
            logger.LogInfo($"Requested find node for {key}");
            await bucketContainer.UpdateBucketOf(sender, cancellationToken);
            return await bucketContainer.FindCloserNodesTo(sender.Id, cancellationToken);
        }
    }
}

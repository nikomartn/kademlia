using Kademlia.Domain.Buckets;
using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Network;
using Logger;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Application.Network
{
    public class PingHandler : IPingHandler
    {
        private readonly BucketContainer bucketContainer;
        private readonly ILogger<PingHandler> logger;

        public PingHandler(BucketContainer bucketContainer, ILogger<PingHandler> logger)
        {
            this.bucketContainer = bucketContainer;
            this.logger = logger;
        }

        public async Task<AknowledgePinged> OnReceivedPing(Contact by, CancellationToken cancellationToken)
        {
            logger.LogInfo("Ping received");
            await bucketContainer.UpdateBucketOf(by, cancellationToken);
            return new AknowledgePinged();
        }
    }
}

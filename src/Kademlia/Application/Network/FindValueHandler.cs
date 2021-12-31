using BinaryStringLib;
using Kademlia.Domain.Buckets;
using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Database;
using Kademlia.Domain.Database.Contracts;
using Kademlia.Domain.Models;
using Kademlia.Domain.Network;
using Logger;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Application.Network
{
    public class FindValueHandler : IFindValueHandler
    {
        private readonly BucketContainer bucketContainer;
        private readonly IDatabase database;
        private readonly ILogger<FindValueHandler> logger;

        public FindValueHandler(BucketContainer bucketContainer, IDatabase database, ILogger<FindValueHandler> logger)
        {
            this.bucketContainer = bucketContainer;
            this.database = database;
            this.logger = logger;
        }

        public async Task<FoundResult> OnReceivedFindValue(Contact sender, string key, CancellationToken cancellationToken)
        {
            logger.LogInfo($"Received FindValue for {key}");
            await bucketContainer.UpdateBucketOf(sender, cancellationToken);
            var result = await database.FindValueForKeyAsync(new BinaryString(key), cancellationToken);
            if (result != null)
            {
                logger.LogInfo($"Found and sending {result}");
                return new FoundResult() { Type = FoundResult.FoundType.FOUND, Data = result };
            }
            else
            {
                logger.LogInfo($"The value was not found, sending clode nodes");
                var closerNodes = await bucketContainer.FindCloserNodesTo(new BinaryString(key), cancellationToken);
                return new FoundResult() { Type = FoundResult.FoundType.FOUND_NODES, Contacts = closerNodes };
            }
        }
    }
}

using Kademlia.Domain.Buckets;
using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Database;
using Kademlia.Domain.Database.Contracts;
using Kademlia.Domain.Network;
using Logger;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Application.Network
{
    public class StoreHandler : IStoreHandler
    {
        private readonly BucketContainer bucketContainer;
        private readonly IDatabase database;
        private readonly ILogger<StoreHandler> logger;

        public StoreHandler(BucketContainer bucketContainer, IDatabase database, ILogger<StoreHandler> logger)
        {
            this.bucketContainer = bucketContainer;
            this.database = database;
            this.logger = logger;
        }

        public async Task<bool> OnReceivedStore(Contact sender, Domain.Database.Contracts.Tuple tuple, CancellationToken cancellationToken)
        {
            logger.LogInfo($"Requested to store {tuple}");
            await bucketContainer.UpdateBucketOf(sender, cancellationToken);
            return await database.StoreAsync(tuple, cancellationToken);
        }
    }
}

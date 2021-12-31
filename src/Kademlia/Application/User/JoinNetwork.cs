using BinaryStringLib.Utils;
using Kademlia.Domain.Buckets;
using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Clock.Events;
using Kademlia.Domain.Interfaces.Client;
using Kademlia.Domain.Iteratives;
using Kademlia.Domain.User;
using Logger;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Application.User
{
    public class JoinNetwork : IJoinNetwork
    {
        private readonly BucketContainer bucketContainer;
        private readonly IterativeFindNode iterativeFindNode;
        private readonly IClient client;
        private readonly ReplicateEvent replicateEvent;
        private readonly ILogger<JoinNetwork> logger;

        public JoinNetwork(BucketContainer bucketContainer,
                           IterativeFindNode iterativeFindNode,
                           IClient client,
                           ReplicateEvent replicateEvent,
                           ILogger<JoinNetwork> logger)
        {
            this.bucketContainer = bucketContainer;
            this.iterativeFindNode = iterativeFindNode;
            this.client = client;
            this.replicateEvent = replicateEvent;
            this.logger = logger;
        }

        public async Task<bool> DoItAsync(string ip, string port, CancellationToken cancellationToken)
        {
            logger.LogInfo($"Requested join network to {ip}:{port}");
            bucketContainer.Me.Id = BinaryStringUtils.GenerateRandomBinaryString(SHA1.Create());

            Contact friend = await client.RequestIdentification(ip, int.Parse(port), cancellationToken);
            if (friend == null)
                return false;
            logger.LogInfo($"Obteined identification {friend}");
            await bucketContainer.UpdateBucketOf(friend, cancellationToken);

            Contact[] nodes = await iterativeFindNode.DoItAsync(bucketContainer.Me.Id, cancellationToken);
            foreach (var node in nodes)
            {
                await bucketContainer.UpdateBucketOf(node, cancellationToken);
            }

            replicateEvent.Setup();

            return true;
        }

    }
}

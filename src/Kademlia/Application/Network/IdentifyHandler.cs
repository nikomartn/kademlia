using Kademlia.Domain.Buckets;
using Kademlia.Domain.Buckets.Contracts;
using Logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Application.Network
{
    public class IdentifyHandler
    {
        private readonly BucketContainer bucketContainer;
        private readonly ILogger<IdentifyHandler> logger;

        public IdentifyHandler(BucketContainer bucketContainer, ILogger<IdentifyHandler> logger)
        {
            this.bucketContainer = bucketContainer;
            this.logger = logger;
        }

        public Task<Contact> OnIdentificationRequested(CancellationToken cancellationToken)
        {
            logger.LogInfo("Sending requested identification");
            return Task.FromResult(bucketContainer.Me);
        }
    }
}

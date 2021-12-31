using Configuration;
using Kademlia.Domain.Clock.Contracts;
using Kademlia.Domain.Database.Contracts;
using Kademlia.Domain.Iteratives;
using Logger;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tuple = Kademlia.Domain.Database.Contracts.Tuple;

namespace Kademlia.Domain.Clock.Events
{
    public class ReplicateEvent
    {
        private readonly IClockManager clockManager;
        private readonly IConfiguration configuration;
        private readonly IDatabase database;
        private readonly IterativeStore iterativeStore;
        private readonly ILogger<ReplicateEvent> logger;

        public ReplicateEvent(IClockManager clockManager, IConfiguration configuration, IDatabase database, IterativeStore iterativeStore, ILogger<ReplicateEvent> logger)
        {
            this.clockManager = clockManager;
            this.configuration = configuration;
            this.database = database;
            this.iterativeStore = iterativeStore;
            this.logger = logger;
        }

        internal void Setup()
        {
            clockManager.Program(this, "Replicate", OnReplicateEvent, int.Parse(configuration["tReplicate"]), LogException);
        }

        private async Task OnReplicateEvent(object _, string ticket, CancellationToken cancellationToken)
        {
            Tuple[] data = await database.CloneData();
            foreach (Tuple t in data)
            {
                await iterativeStore.StoreAsync(t, cancellationToken);
            }
        }

        public void LogException(Exception e)
        {
            logger.LogWarning($"Exception::{e}");
        }

    }
}

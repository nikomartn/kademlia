using BinaryStringLib;
using Configuration;
using Kademlia.Domain.Clock.Contracts;
using Kademlia.Domain.Iteratives;
using Logger;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tuple = Kademlia.Domain.Database.Contracts.Tuple;

namespace Kademlia.Domain.Clock.Events
{
    public class RepublishEvent
    {
        private readonly IClockManager clockManager;
        private readonly IterativeStore iterativeStore;
        private readonly IConfiguration configuration;
        private readonly ILogger<RepublishEvent> logger;

        public RepublishEvent(IClockManager clockManager, IterativeStore iterativeStore, IConfiguration configuration, ILogger<RepublishEvent> logger)
        {
            this.clockManager = clockManager;
            this.iterativeStore = iterativeStore;
            this.configuration = configuration;
            this.logger = logger;
        }

        public void CreateEvent(Tuple tuple)
        {
            clockManager.Program
            (
                this,
                $"Republish/{tuple.Key}/{tuple.Value}",
                OnRepublishEvent,
                int.Parse(configuration["tRepublish"]),
                LogException
            );
        }

        private void LogException(Exception obj)
        {
            logger.LogWarning($"Exception::{obj}");
        }

        private Task OnRepublishEvent(object _, string ticket, CancellationToken cancellationToken)
        {
            var data = ticket.Split('/');
            return iterativeStore.StoreAsync(new Tuple()
            {
                Key = new BinaryString(data[1]),
                Value = data[2]
            }, cancellationToken);
        }
    }
}

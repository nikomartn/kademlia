using BinaryStringLib;
using Configuration;
using Kademlia.Domain.Clock.Contracts;
using Kademlia.Domain.Database.Contracts;
using Logger;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Domain.Clock.Events
{
    public class ExpireEvent
    {
        private readonly IClockManager clockManager;
        private readonly IDatabase database;
        private readonly IConfiguration configuration;
        private readonly ILogger<ExpireEvent> logger;

        public ExpireEvent(IClockManager clockManager, IDatabase database, IConfiguration configuration, ILogger<ExpireEvent> logger)
        {
            this.clockManager = clockManager;
            this.database = database;
            this.configuration = configuration;
            this.logger = logger;
        }

        internal void SetExpirationFor(Database.Contracts.Tuple tuple)
        {
            clockManager.Program(this, $"Expire/{tuple.Key}", OnExpired, int.Parse(configuration["tExpire"]), LogException);
        }

        private void LogException(Exception obj)
        {
            logger.LogWarning($"Exception::{obj}");
        }

        private Task OnExpired(object sender, string ticket, CancellationToken cancellationToken)
        {
            BinaryString key = new BinaryString(ticket.Split('/')[1]);
            return database.Remove(key, cancellationToken);
        }

        internal void RemoveExpirationFor(Database.Contracts.Tuple tuple)
        {
            clockManager.RemoveProgramming(this, $"Expire/{tuple.Key}");
        }
    }
}

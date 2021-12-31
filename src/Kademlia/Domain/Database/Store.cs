using Configuration;
using Kademlia.Domain.Clock.Contracts;
using Kademlia.Domain.Clock.Events;
using Kademlia.Domain.Database.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Tuple = Kademlia.Domain.Database.Contracts.Tuple;

namespace Kademlia.Domain.Database
{
    public class Store
    {
        private readonly IDatabase database;
        private readonly ExpireEvent expireEvent;

        public Store(IDatabase database, IClockManager clockManager, IConfiguration configuration, ExpireEvent expireEvent)
        {
            this.database = database;
            this.expireEvent = expireEvent;
        }

        public async Task<bool> DoItAsync(Tuple tuple, CancellationToken cancellationToken)
        {
            if (await database.FindValueForKeyAsync(tuple.Key, cancellationToken) != null)
                expireEvent.RemoveExpirationFor(tuple);

            expireEvent.SetExpirationFor(tuple);
            return await database.StoreAsync(tuple, cancellationToken);
        }
    }
}

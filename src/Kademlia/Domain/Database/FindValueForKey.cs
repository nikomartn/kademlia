using BinaryStringLib;
using Kademlia.Domain.Database.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Domain.Database
{
    public class FindValueForKey
    {
        private readonly IDatabase database;

        public FindValueForKey(IDatabase database)
        {
            this.database = database;
        }

        public Task<Contracts.Tuple> DoItAsync(string key, CancellationToken cancellationToken)
        {
            return database.FindValueForKeyAsync(new BinaryString(key), cancellationToken);
        }
    }
}

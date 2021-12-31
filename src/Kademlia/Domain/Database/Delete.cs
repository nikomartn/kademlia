using BinaryStringLib;
using Kademlia.Domain.Database.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Domain.Database
{
    public class Delete
    {
        private readonly IDatabase database;

        public Delete(IDatabase database)
        {
            this.database = database;
        }

        internal Task DoItAsync(string key, CancellationToken cancellationToken)
        {
            return database.Remove(new BinaryString(key), cancellationToken);
        }
    }
}

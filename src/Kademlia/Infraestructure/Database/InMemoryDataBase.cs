using BinaryStringLib;
using Kademlia.Domain.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tuple = Kademlia.Domain.Database.Contracts.Tuple;

namespace Kademlia.Infraestructure.Database
{
    public class InMemoryDataBase : IDatabase
    {

        protected Dictionary<string, string> database = new Dictionary<string, string>();

        public Task<Tuple[]> CloneData()
        {
            return Task.Run(() =>
            {
                List<Tuple> copy = new List<Domain.Database.Contracts.Tuple>();
                lock (database)
                {
                    foreach (var key in database.Keys)
                    {
                        copy.Add(new Tuple() { Key = new BinaryString(key), Value = database[key] });
                    }
                }
                return copy.ToArray();
            });
        }

        public Task<Tuple> FindValueForKeyAsync(BinaryString key, CancellationToken cancellationToken)
        {
            return Task.Run<Tuple>(() =>
            {
                lock (database)
                {
                    if (!database.ContainsKey(key.StringHex))
                        return null;

                    return (new Tuple() { Key = key, Value = database[key.StringHex] });
                }
            });
        }

        public Task Remove(BinaryString key, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                lock (database)
                {
                    if (database.ContainsKey(key.StringHex))
                        database.Remove(key.StringHex);
                }
            });
        }

        public Task<bool> StoreAsync(Tuple tuple, CancellationToken cancellationToken)
        {
            database[tuple.Key.StringHex] = tuple.Value;
            return Task.FromResult(true);
        }
    }
}

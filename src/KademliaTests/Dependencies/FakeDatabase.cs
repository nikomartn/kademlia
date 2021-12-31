using BinaryStringLib;
using Kademlia.Domain.Database.Contracts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KademliaTests.Dependencies
{
    internal class FakeDatabase : IDatabase
    {
        public FakeDatabase()
        {
        }

        public Dictionary<string, Tuple> Data = new Dictionary<string, Tuple>();

        public Task<Tuple[]> CloneData()
        {
            Tuple[] clone = new Tuple[Data.Values.Count];
            Data.Values.CopyTo(clone, 0);
            return Task.FromResult<Tuple[]>(clone);
        }

        public Task<Tuple> FindValueForKeyAsync(BinaryString key, CancellationToken cancellationToken)
        {
            if (Data.ContainsKey(key.StringHex))
                return Task.FromResult(Data[key.StringHex]);
            return Task.FromResult<Tuple>(null);
        }

        public Task Remove(BinaryString key, CancellationToken cancellationToken)
        {
            if (Data.ContainsKey(key.StringHex))
                Data.Remove(key.StringHex);
            return Task.Delay(0);
        }

        public Task<bool> StoreAsync(Tuple tuple, CancellationToken cancellationToken)
        {
            Data[tuple.Key.StringHex] = tuple;
            return Task.FromResult(true);
        }
    }
}
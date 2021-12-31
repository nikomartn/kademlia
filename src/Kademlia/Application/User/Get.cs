using BinaryStringLib;
using Kademlia.Domain.Iteratives;
using System.Threading;
using System.Threading.Tasks;
using Tuple = Kademlia.Domain.Database.Contracts.Tuple;
namespace Kademlia.Application.User
{
    public class Get
    {
        private readonly IterativeFindValue iterativeFindValue;

        public Get(IterativeFindValue iterativeFindValue)
        {
            this.iterativeFindValue = iterativeFindValue;
        }

        public async Task<Tuple> DoItAsync(string key, CancellationToken cancellationToken)
        {
            Tuple tuple = await iterativeFindValue.DoItAsync(new BinaryString(key), cancellationToken);
            return tuple;
        }
    }
}

using BinaryStringLib;
using Kademlia.Domain.Buckets.Contracts;
using System.Collections.Generic;

namespace Kademlia.Domain.Buckets
{
    public class CloserToComparer : IComparer<Contact>
    {
        private readonly BinaryString key;

        public CloserToComparer(BinaryString key)
        {
            this.key = key;
        }

        public int Compare(Contact x, Contact y)
        {
            if ((x.Id ^ key) < (y.Id ^ key))
                return -1;
            else if ((x.Id ^ key) > (y.Id ^ key))
                return 1;
            return 0;
        }
    }
}

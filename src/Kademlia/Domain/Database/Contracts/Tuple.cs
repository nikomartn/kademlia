using BinaryStringLib;

namespace Kademlia.Domain.Database.Contracts
{
    public class Tuple
    {

        public Tuple() { }

        public Tuple(string tuple)
        {
            var data = tuple.Split(',');
            Key = new BinaryString(data[0]);
            Value = data[1];
        }

        public override string ToString()
        {
            return $"{Key},{Value}";
        }

        public BinaryString Key { get; set; }
        public string Value { get; set; }
    }
}

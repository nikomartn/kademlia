using BinaryStringLib;

namespace Kademlia.Domain.Buckets.Contracts
{
    public class Contact
    {
        public BinaryString Id { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }

        public Contact() { }

        public Contact(string str)
        {
            var data = str.Split(',');
            Id = new BinaryString(data[0]);
            Ip = data[1];
            Port = int.Parse(data[2]);
        }

        public override string ToString()
        {
            return $"{Id.StringHex},{Ip},{Port}";
        }
    }
}

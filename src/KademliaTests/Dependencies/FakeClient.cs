using BinaryStringLib;
using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Database.Contracts;
using Kademlia.Domain.Interfaces.Client;
using Kademlia.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace KademliaTests.Dependencies
{
    internal class FakeClient : IClient
    {

        public const string ServerFakeId = "ABCD";
        public const string ServerFakeIp = "255.255.255.255";
        public const string ServerFakePort = "255";
        public const string FakeServerContact = "ABCD,255.255.255.255,255";

        public const string FakeTuple = "AAAA,wabalabadabdab";

        public Task<FoundResult> FindValue(Contact sender, Contact contact, BinaryString id, CancellationToken cancellationToken)
        {
            if (id.StringHex == "AAAA")
            {
                return Task.FromResult(new FoundResult() {Type = FoundResult.FoundType.FOUND, Data = new Tuple(FakeTuple) });
            }
            else
            {
                return Task.FromResult(
                    new FoundResult() { Type = FoundResult.FoundType.FOUND_NODES, Contacts = 
                    new Contact[] { new Contact(FakeKnownContact) } });
            }
        }

        public Task<bool> MakePing(Contact sender, Contact contact, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public string LastRequestIdentification { get; protected set; }

        public Task<Contact> RequestIdentification(string ip, int port, CancellationToken cancellationToken)
        {
            LastRequestIdentification = $"{ip}/{port}";
            return Task.FromResult(new Contact($"{ServerFakeId},{ip},{port}"));
        }

        public string LastStoreRequest { get; protected set; }

        public Task Store(Contact sender, Contact contact, Tuple tuple, CancellationToken cancellationToken)
        {
            LastStoreRequest = $"{contact}/{tuple}";
            return Task.Delay(0);
        }

        public string LastRequestGetNodes { get; protected set; }
        public const string FakeKnownContact = "AAAA,254.254.254.254,0";

        public Task<Contact[]> FindNodes(Contact sender, Contact contact, BinaryString id, CancellationToken cancellationToken)
        {
            LastRequestGetNodes = $"{contact}";
            if (contact.ToString() == FakeServerContact)
                return Task.FromResult(new Contact[] { new Contact(FakeKnownContact) });

            return Task.FromResult(new Contact[] { });
        }

    }
}
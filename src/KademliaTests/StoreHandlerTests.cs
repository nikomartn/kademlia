using Kademlia.Domain.Buckets.Contracts;
using KademliaTests.Dependencies;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tuple = Kademlia.Domain.Database.Contracts.Tuple;
namespace KademliaTests
{
    public class StoreHandlerTests
    {
        [Test]
        public async Task Store()
        {
            var client = new FakeClient();
            var configuration = new FakeConfiguration();
            configuration.GenerateMocking();
            var database = new FakeDatabase();

            var mother = new InstanceMother(client, configuration, database);
            await mother.JoinNetwork.DoItAsync("255.255.255.255", "255", CancellationToken.None);

            Tuple t = new Tuple("AAAA,wabalabadabdab");
            bool result = await mother.StoreHandler.OnReceivedStore(new Contact(FakeClient.FakeServerContact), t, CancellationToken.None);

            Assert.IsTrue(result);

            Assert.AreEqual(t.Key.StringHex, database.Data["AAAA"].Key.StringHex);
        }
    }
}

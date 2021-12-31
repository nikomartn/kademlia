using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Network;
using KademliaTests.Dependencies;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KademliaTests
{
    public class PingTest
    {
        [Test]
        public async Task Ping()
        {
            var client = new FakeClient();
            var configuration = new FakeConfiguration();
            configuration.GenerateMocking();
            var database = new FakeDatabase();

            var mother = new InstanceMother(client, configuration, database);

            var res = await mother.PingHandler.OnReceivedPing(new Contact(FakeClient.FakeServerContact), CancellationToken.None);

            Assert.AreEqual(typeof(AknowledgePinged), res.GetType());
        }
    }
}

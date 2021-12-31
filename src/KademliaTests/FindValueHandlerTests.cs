using BinaryStringLib;
using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Models;
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
    public class FindValueHandlerTests
    {
        [Test]
        public async Task FromDatabase()
        {
            var client = new FakeClient();
            var configuration = new FakeConfiguration();
            configuration.GenerateMocking();
            var database = new FakeDatabase();

            var mother = new InstanceMother(client, configuration, database);
            await mother.JoinNetwork.DoItAsync("255.255.255.255", "255", CancellationToken.None);

            database.Data["AAAA"] = new Tuple() { Key = new BinaryString("AAAA"), Value = "Wabalabadabdab" };

            var found = await mother.FindValueHandler.OnReceivedFindValue(new Contact(FakeClient.FakeServerContact), "AAAA", CancellationToken.None);

            Assert.AreEqual(FoundResult.FoundType.FOUND, found.Type);

            Assert.AreEqual("AAAA", found.Data.Key.StringHex);
        }

        [Test]
        public async Task FromNetwork()
        {
            var client = new FakeClient();
            var configuration = new FakeConfiguration();
            configuration.GenerateMocking();
            var database = new FakeDatabase();

            var mother = new InstanceMother(client, configuration, database);
            await mother.JoinNetwork.DoItAsync("255.255.255.255", "255", CancellationToken.None);

            var found = await mother.FindValueHandler.OnReceivedFindValue(new Contact(FakeClient.FakeServerContact), "AAAA", CancellationToken.None);

            Assert.AreEqual(FoundResult.FoundType.FOUND_NODES, found.Type);

        }

    }
}

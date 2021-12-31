using BinaryStringLib;
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
    public class GetTests
    {
        [Test]
        public async Task GetFromDatabase()
        {
            var client = new FakeClient();
            var configuration = new FakeConfiguration();
            configuration.GenerateMocking();
            var database = new FakeDatabase();

            var mother = new InstanceMother(client, configuration, database);

            var get = mother.Get;

            Tuple t = new Tuple() { Key = new BinaryString("AAAA"), Value = "Wabalabadubdub" };

            database.Data.Add(t.Key.StringHex, t);

            var result = await get.DoItAsync("AAAA", CancellationToken.None);

            Assert.AreEqual(t.ToString(), result.ToString());
        }

        [Test]
        public async Task GetFromNetwork()
        {
            var client = new FakeClient();
            var configuration = new FakeConfiguration();
            configuration.GenerateMocking();
            var database = new FakeDatabase();

            var mother = new InstanceMother(client, configuration, database);
            await mother.JoinNetwork.DoItAsync("255.255.255.255", "255", CancellationToken.None);
            var get = mother.Get;

            var result = await get.DoItAsync("AAAA", CancellationToken.None);

            Assert.AreEqual(FakeClient.FakeTuple, result.ToString());
        }
    }
}

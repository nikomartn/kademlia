using Kademlia.Domain.Buckets.Contracts;
using KademliaTests.Dependencies;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KademliaTests
{
    public class FindNodeHandlerTests
    {
        [Test]
        public async Task AbleToFindNodes()
        {
            var client = new FakeClient();
            var configuration = new FakeConfiguration();
            configuration.GenerateMocking();
            var database = new FakeDatabase();

            var mother = new InstanceMother(client, configuration, database);
            await mother.JoinNetwork.DoItAsync("255.255.255.255", "255", CancellationToken.None);

            var response = await mother.FindNodeHandler.OnReceivedFindNode
            (
                new Contact(FakeClient.FakeServerContact),
                FakeClient.ServerFakeId,
                CancellationToken.None
            );

            List<string> stringResponse = new List<string>();
            foreach(Contact c in response)
            {
                stringResponse.Add(c.ToString());
            }

            var fakeServerIsThere = stringResponse.Find((c) => c == FakeClient.FakeServerContact);
            Assert.NotNull(fakeServerIsThere);

            var fakeKnownIsThere = stringResponse.Find((c) => c == FakeClient.FakeKnownContact);
            Assert.NotNull(fakeKnownIsThere);
        }
    }
}

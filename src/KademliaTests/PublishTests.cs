using BinaryStringLib;
using Kademlia.Domain.Database.Contracts;
using KademliaTests.Dependencies;
using NUnit.Framework;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace KademliaTests
{
    public class PublishTests
    {
        /// <summary>
        /// Publish data
        /// 
        /// @ Join a network
        /// @ Publish some data
        /// @ Receive the generated key
        /// @ Request store to the closest nodes
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Publish()
        {
            var client = new FakeClient();
            var configuration = new FakeConfiguration();
            configuration.GenerateMocking();
            var database = new FakeDatabase();

            var mother = new InstanceMother(client, configuration, database);

            var publish = mother.Publish;

            string v = "Hola mundo";
            byte[] data = Encoding.ASCII.GetBytes(v);

            await mother.JoinNetwork.DoItAsync(FakeClient.ServerFakeIp, FakeClient.ServerFakePort, CancellationToken.None);

            string key = await publish.DoIt(v, CancellationToken.None);
            Tuple t = new Tuple() { Key = new BinaryString(key), Value = v };

            Assert.IsTrue(client.LastStoreRequest == $"{FakeClient.FakeKnownContact}/{t}"
                || client.LastStoreRequest == $"{FakeClient.FakeServerContact}/{t}", $"{client.LastStoreRequest}");

        }
    }
}

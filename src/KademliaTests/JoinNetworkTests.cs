using BinaryStringLib;
using KademliaTests.Dependencies;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace KademliaTests
{
    public class JoinNetworkTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Intanciates()
        {
            FakeConfiguration fakeConfiguration = new FakeConfiguration();
            fakeConfiguration["Ip"] = "127.0.0.1";
            fakeConfiguration["Port"] = "0";
            FakeClient fakeClient = new FakeClient();
            FakeDatabase fakeDatabase = new FakeDatabase();
            var mother = new InstanceMother(fakeClient, fakeConfiguration, fakeDatabase);
            Assert.NotNull(mother);
        }

        /// <summary>
        /// To Join a network
        /// 
        /// Given an IP and Port, it should 
        /// @ Generate an ID
        /// @ Request the node id of that node
        /// @ Store that id (It should be posible to obtain it back)
        /// @ Request closer nodes to own ID
        /// @ Store them
        /// @ Program a tRepublish event
        /// </summary>
        [Test]
        public async Task JoinNetwork()
        {
            FakeConfiguration fakeConfiguration = new FakeConfiguration();
            fakeConfiguration["Ip"] = "127.0.0.1";
            fakeConfiguration["Port"] = "0";
            fakeConfiguration["tRefresh"] = "9999999";
            fakeConfiguration["tReplicate"] = "99999999";
            fakeConfiguration["K"] = "20";
            FakeClient fakeClient = new FakeClient();
            FakeDatabase fakeDatabase = new FakeDatabase();
            var mother = new InstanceMother(fakeClient, fakeConfiguration, fakeDatabase);

            await mother.JoinNetwork.DoItAsync(FakeClient.ServerFakeIp, FakeClient.ServerFakePort, CancellationToken.None);

            // @ Generates ID
            Assert.NotNull(mother.BucketContainer.Me.Id, "Me.Id was null");
            Assert.AreEqual("127.0.0.1", mother.BucketContainer.Me.Ip, "Me.Ip different than selected");
            Assert.AreEqual(0, mother.BucketContainer.Me.Port, "Me.Port different than selected");

            // @ Requests ID to the friend
            Assert.AreEqual(
                $"{FakeClient.ServerFakeIp}/{FakeClient.ServerFakePort}",
                fakeClient.LastRequestIdentification, "Last requested identification was not the friend");

            // @ Stores them all

            var list = (await mother.BucketContainer.FindCloserNodesTo
                (
                    new BinaryString(FakeClient.ServerFakeId),
                    CancellationToken.None
                ));
            Assert.AreEqual(2, list.Length);

            var foundedCloserNodeToServerContact = list[0].ToString();
            var foundedKnownContact = list[1].ToString();

            Assert.AreEqual
            (
                FakeClient.FakeServerContact,
                foundedCloserNodeToServerContact,
                "It did not got the nodes provided by the friend"
            );

            Assert.AreEqual(FakeClient.FakeKnownContact, foundedKnownContact,
                "The node given by the friend was not stored");

        }
    }
}
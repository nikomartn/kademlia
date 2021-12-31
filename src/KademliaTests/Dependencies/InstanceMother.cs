using Configuration;
using Kademlia.Application.Network;
using Kademlia.Application.User;
using Kademlia.Domain.Buckets;
using Kademlia.Domain.Clock;
using Kademlia.Domain.Clock.Events;
using Kademlia.Domain.Database.Contracts;
using Kademlia.Domain.Interfaces.Client;
using Kademlia.Domain.Iteratives;
using Logger;

namespace KademliaTests.Dependencies
{
    public class InstanceMother
    {
        public InstanceMother(IClient fakeClient, IConfiguration fakeConfiguration, IDatabase fakeDatabase)
        {
            FakeClient = fakeClient;
            FakeConfiguration = fakeConfiguration;
            FakeDatabase = fakeDatabase;
            ClockManager = new ClockManager();
            LoggerRepo lr = new LoggerRepo();
            BucketContainer = new BucketContainer(FakeConfiguration, FakeClient, ClockManager, new Logger<BucketContainer>(lr));
            IterativeFindNode = new IterativeFindNode(FakeClient, BucketContainer, new Logger<IterativeFindNode>(lr));
            IterativeFindValue = new IterativeFindValue(BucketContainer, fakeConfiguration, fakeClient, fakeDatabase, new Logger<IterativeFindValue>(lr));
            IterativeStore = new IterativeStore(IterativeFindNode, FakeClient, BucketContainer);
            ReplicateEvent = new ReplicateEvent(ClockManager, FakeConfiguration, FakeDatabase, IterativeStore, new Logger<ReplicateEvent>(lr));
            RepublishEvent = new RepublishEvent(ClockManager, IterativeStore, FakeConfiguration, new Logger<RepublishEvent>(lr));
            JoinNetwork = new JoinNetwork(BucketContainer, IterativeFindNode, FakeClient, ReplicateEvent, new Logger<JoinNetwork>(lr));
            Publish = new Publish(IterativeStore, RepublishEvent, new Logger<Publish>(lr));
            Get = new Get(IterativeFindValue);
            PingHandler = new PingHandler(BucketContainer, new Logger<PingHandler>(lr));
            FindNodeHandler = new FindNodeHandler(BucketContainer, new Logger<FindNodeHandler>(lr));
            FindValueHandler = new FindValueHandler(BucketContainer, FakeDatabase, new Logger<FindValueHandler>(lr));
            StoreHandler = new StoreHandler(BucketContainer, FakeDatabase, new Logger<StoreHandler>(lr));
        }

        public IClient FakeClient { get; }
        public IConfiguration FakeConfiguration { get; }
        public IDatabase FakeDatabase { get; }
        public ClockManager ClockManager { get; }
        public BucketContainer BucketContainer { get; }
        public IterativeFindNode IterativeFindNode { get; }
        public IterativeFindValue IterativeFindValue { get; }
        public IterativeStore IterativeStore { get; }
        public ReplicateEvent ReplicateEvent { get; }
        public RepublishEvent RepublishEvent { get; }
        public JoinNetwork JoinNetwork { get; }
        public Publish Publish { get; }
        public Get Get { get; }
        public PingHandler PingHandler { get; }
        public FindNodeHandler FindNodeHandler { get; }
        public FindValueHandler FindValueHandler { get; }
        public StoreHandler StoreHandler { get; }
    }
}

using BinaryStringLib;
using Configuration;
using Kademlia.Domain.Buckets;
using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Interfaces.Client;
using Kademlia.Domain.Iteratives.Exception;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Domain.Iteratives
{
    public class IterativeFindNode
    {
        private readonly IClient client;
        private readonly BucketContainer bucketContainer;
        private readonly ILogger<IterativeFindNode> logger;

        public IterativeFindNode(IClient client, BucketContainer bucketContainer, ILogger<IterativeFindNode> logger)
        {
            this.client = client;
            this.bucketContainer = bucketContainer;
            this.logger = logger;
        }

        private class IterativeFindAlgorithm
        {
            private readonly BinaryString id;
            private readonly IClient client;
            private readonly BucketContainer bucketContainer;
            readonly CloserToComparer sorter;
            readonly List<Contact> candidates = new List<Contact>();
            readonly List<Contact> shortList = new List<Contact>();

            public IterativeFindAlgorithm(List<Contact> startList, BinaryString id, IClient client, BucketContainer bucketContainer)
            {
                this.id = id ?? throw new ArgumentNullException(nameof(id));
                this.client = client;
                this.bucketContainer = bucketContainer;
                sorter = new CloserToComparer(id);

                if (startList is null)
                {
                    throw new ArgumentNullException(nameof(startList));
                }

                candidates.AddRange(startList);
                candidates.Sort(sorter);
            }

            private bool ShortListIs20 => shortList.Count == 20;
            private bool NoBetterCandidate
            {
                get
                {
                    if (candidates.Count > 0)
                    {
                        if (shortList.Count == 0)
                            return false;
                        return (shortList.Last().Id ^ id) < (candidates.First().Id ^ id);
                    }
                    return true;
                }
            }

            private void AddCandidate(Contact c)
            { // Only not contacted nodes can be candidates
                if (!shortList.Exists(cc => cc.Id.StringHex == c.Id.StringHex) &&
                    !candidates.Exists(cc => cc.Id.StringHex == c.Id.StringHex))
                {
                    candidates.Add(c);
                    candidates.Sort(sorter);
                }
                
            }

            // Add a contact to shortList, mantaining the invariant that shortList.Count < 20
            private void MoveToShortList(Contact c)
            {
                candidates.Remove(c);
                shortList.Add(c);
                shortList.Sort(sorter);
                while (shortList.Count > 20)
                {
                    shortList.Remove(shortList.Last());
                }
            }

            // Get up to 3 best contacts from candidates
            private List<Contact> GetAlpha()
            {
                List<Contact> alpha = new List<Contact>();
                foreach (Contact c in candidates)
                {
                    if (alpha.Count == 3)
                        return alpha;
                    alpha.Add(c);
                }
                return alpha;
            }

            public async Task<List<Contact>> FindBestContacts(CancellationToken cancellationToken)
            {
                while (candidates.Count > 0)
                {
                    if (ShortListIs20 && NoBetterCandidate)
                    {
                        return shortList;
                    }

                    var alpha = GetAlpha();
                    foreach (Contact c in alpha)
                    {
                        List<Contact> response = new List<Contact>
                        (
                            (
                                await client.FindNodes(bucketContainer.Me, c, id, cancellationToken)
                            ).Where((contact) => contact.Id.StringHex != bucketContainer.Me.Id.StringHex)
                        );
                        
                        Contact notMe;
                        while((notMe = response.FirstOrDefault((c) => c.Id.StringHex == bucketContainer.Me.Id.StringHex)) != null)
                        {
                            response.Remove(notMe);
                        }
                        
                        await bucketContainer.UpdateBucketOf(c, cancellationToken);
                        foreach (Contact newest in response)
                        {
                            AddCandidate(newest);
                        }
                        MoveToShortList(c);
                        
                    }
                }
                return shortList;
            }
        }

        public async Task<Contact[]> DoItAsync(BinaryString id, CancellationToken cancellationToken)
        {
            logger.LogInfo($"IterativeFindingNodes for {id}");
            var knownsBeforeHand = await bucketContainer.FindCloserNodesTo(id, cancellationToken);
            if (knownsBeforeHand.Length == 0)
                throw new NotKnownNodesAvailable();

            var algorithm = new IterativeFindAlgorithm(new List<Contact>(knownsBeforeHand), id, client, bucketContainer);
            var contactedAndClosestNodes = await algorithm.FindBestContacts(cancellationToken);
            logger.LogInfo($"Found nodes for {id}");
            return contactedAndClosestNodes.ToArray();
        }




    }
}

using BinaryStringLib;
using BinaryStringLib.Utils;
using Configuration;
using Kademlia.Domain.Buckets.Contracts;
using Kademlia.Domain.Clock.Contracts;
using Kademlia.Domain.Interfaces.Client;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Domain.Buckets
{
    public class BucketContainer
    {
        private readonly IConfiguration configuration;
        private readonly IClient client;
        private readonly IClockManager clockManager;
        private readonly ILogger<BucketContainer> logger;

        public Contact Me { get; protected set; }

        private class Bucket
        {
            public DateTime lastUpdated = DateTime.Now;
            public List<Contact> contacts = new List<Contact>();
            public SemaphoreSlim semaphore = new SemaphoreSlim(1);
        }

        Bucket[] buckets = new Bucket[160];

        public BucketContainer(IConfiguration configuration, IClient client, IClockManager clockManager, ILogger<BucketContainer> logger)
        {
            this.configuration = configuration;
            this.client = client;
            this.clockManager = clockManager;
            this.logger = logger;
            Me = new Contact()
            {
                Id = BinaryStringUtils.GenerateRandomBinaryString(SHA1.Create()),
                Ip = configuration["Ip"],
                Port = int.Parse(configuration["Port"])
            };
            for (int i = 0; i < 160; i++)
            {
                buckets[i] = new Bucket();
            }
        }

        public async Task<Contact[]> FindCloserNodesTo(BinaryString of, CancellationToken cancellationToken)
        {
            // Get a copy of all contacts
            List<Contact> all = new List<Contact>();
            await Task.Run(() =>
            {
                foreach (Bucket b in buckets)
                {
                    try
                    {
                        b.semaphore.Wait();
                        all.AddRange(b.contacts.ToArray());
                    }
                    finally
                    {
                        b.semaphore.Release();
                    }
                }
            });

            // Remove remitent if it's on this contacts 
            // TODO: It can not be the remmitent, but a Key, and it is posible to a node to have an identical key.
            /*var ofInAll = all.Find((contact) => contact.Id == of);
            if (ofInAll != null)
                all.Remove(ofInAll);*/

            all.Sort(new CloserToComparer(of));

            // If the list is smaller than K, return
            if (all.Count() <= int.Parse(configuration["K"]))
                return all.ToArray();

            // Else, return closer nodes to remitent
            await Task.Run(() => all.Sort(new CloserToComparer(of)));
            return all.GetRange(0, int.Parse(configuration["K"])).ToArray();
        }



        public async Task UpdateBucketOf(Contact contact, CancellationToken cancellationToken)
        {
            var distance = contact.Id ^ Me.Id;
            int bucketNumber = (distance.BitSize) - 1;

            if (contact.Id.StringHex == Me.Id.StringHex)
                return;
            logger.LogInfo($"Updating bucket of {contact}");

            try
            {
                buckets[bucketNumber].semaphore.Wait();
                if (buckets[bucketNumber].contacts.Exists((c) => contact.Id.StringHex == c.Id.StringHex))
                {
                    var item = buckets[bucketNumber].contacts.First((c) => contact.Id.StringHex == c.Id.StringHex);
                    buckets[bucketNumber].contacts.Remove(item);
                    buckets[bucketNumber].contacts.Add(item);
                }
                else
                {
                    if (buckets[bucketNumber].contacts.Count() < 20)
                    {
                        buckets[bucketNumber].contacts.Add(contact);
                    }
                    else
                    {
                        var old = buckets[bucketNumber].contacts[0];
                        bool responded = await client.MakePing(Me ,old, cancellationToken);
                        buckets[bucketNumber].contacts.Remove(old);

                        if (responded)
                            buckets[bucketNumber].contacts.Add(old);
                        else
                            buckets[bucketNumber].contacts.Add(contact);
                    }
                }
                if (buckets[bucketNumber].contacts.Count > 0)
                {
                    ReprogramUpdateForBucket(bucketNumber);
                }
            }
            finally
            {
                buckets[bucketNumber].semaphore.Release();
            }
        }

        public async Task OnRefreshBucket(object sender, string ticket, CancellationToken cancellationToken)
        {
            var bucket = int.Parse(ticket.Split('/')[1]);
            try
            {
                await UpdateBucket(bucket, cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogWarning($"Unable to refresh bucket {e}");
            }
        }

        private async Task UpdateBucket(int bucket, CancellationToken cancellationToken)
        {
            logger.LogInfo($"Updating bucket:{bucket}");
            try
            {
                buckets[bucket].semaphore.Wait();
                if (buckets[bucket].contacts.Count > 0)
                {
                    Contact oldest = buckets[bucket].contacts[0];
                    buckets[bucket].contacts.Remove(oldest);
                    bool responded = await client.MakePing(Me, oldest, cancellationToken);
                    if (responded)
                        buckets[bucket].contacts.Add(oldest);
                }
                if (buckets[bucket].contacts.Count > 0)
                {
                    ReprogramUpdateForBucket(bucket);
                }
            }
            finally
            {
                buckets[bucket].semaphore.Release();
            }
        }

        private void ReprogramUpdateForBucket(int bucketNumber)
        {
            logger.LogInfo($"Reprograming update for bucket:{bucketNumber}");
            clockManager.RemoveProgramming(this, $"Refresh bucket {bucketNumber}");
            clockManager.Program(this, $"Refresh bucket /{bucketNumber}", OnRefreshBucket, int.Parse(configuration["tRefresh"]));
        }
    }
}

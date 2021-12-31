using BinaryStringLib;
using Kademlia.Domain.Clock.Events;
using Kademlia.Domain.Iteratives;
using Kademlia.Domain.User;
using Logger;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tuple = Kademlia.Domain.Database.Contracts.Tuple;

namespace Kademlia.Application.User
{
    public class Publish : IPublish
    {
        private readonly IterativeStore iterativeStore;
        private readonly RepublishEvent republishEvent;
        private readonly ILogger<Publish> logger;

        public Publish(IterativeStore iterativeStore, RepublishEvent republishEvent, ILogger<Publish> logger)
        {
            this.iterativeStore = iterativeStore;
            this.republishEvent = republishEvent;
            this.logger = logger;
        }
        public async Task<string> DoIt(string data, CancellationToken cancellationToken)
        {
            var key = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(data));
            var keyHex = Regex.Replace(BitConverter.ToString(key), "-", String.Empty);
            var tuple = new Tuple()
            {
                Key = new BinaryString(keyHex),
                Value = data
            };
            logger.LogInfo($"Publishing data {tuple}");

            await iterativeStore.StoreAsync(tuple, cancellationToken);
            republishEvent.CreateEvent(tuple);
            return keyHex;
        }
    }
}

using Kademlia.Application.Network;
using Kademlia.Domain.Buckets.Contracts;
using Router.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Kademlia.Domain.Models.FoundResult;
using Tuple = Kademlia.Domain.Database.Contracts.Tuple;
namespace Router.Infraestructure
{
    public class HandlerProvider : IHandlerProvider
    {
        private readonly FindNodeHandler findNodeHandler;
        private readonly FindValueHandler findValueHandler;
        private readonly PingHandler pingHandler;
        private readonly StoreHandler storeHandler;
        private readonly IdentifyHandler identifyHandler;

        public HandlerProvider(FindNodeHandler findNodeHandler,
                               FindValueHandler findValueHandler,
                               PingHandler pingHandler,
                               StoreHandler storeHandler,
                               IdentifyHandler identifyHandler)
        {
            this.findNodeHandler = findNodeHandler;
            this.findValueHandler = findValueHandler;
            this.pingHandler = pingHandler;
            this.storeHandler = storeHandler;
            this.identifyHandler = identifyHandler;
        }

        public async Task<string> RouteToFindNode(string[] data, CancellationToken cancellationToken)
        {
            var contacts = await findNodeHandler.OnReceivedFindNode(new Contact(data[1]), data[2], cancellationToken);
            string response = "FOUND_NODES";
            foreach(var contact in contacts)
            {
                response += $";{contact}";
            }
            return response;
        }

        public async Task<string> RouteToFindValue(string[] data, CancellationToken cancellationToken)
        {
            var found = await findValueHandler.OnReceivedFindValue(new Contact(data[1]), data[2], cancellationToken);
            if (found.Type == FoundType.FOUND)
            {
                return $"FOUND_VALUE;VALUE;{found.Data.Value}";
            }
            var response = "FOUND_VALUE;FOUND_NODES";
            foreach(var contact in found.Contacts)
            {
                response += $";{contact}";
            }
            return response;
        }

        public async Task<string> RouteToIdentify(string[] data, CancellationToken cancellationToken)
        {
            return $"IDENTIFICATION;{await identifyHandler.OnIdentificationRequested(cancellationToken)}";
        }

        public async Task<string> RouteToPing(string[] data, CancellationToken cancellationToken)
        {
            _ = await pingHandler.OnReceivedPing(new Contact(data[1]), cancellationToken);
            return "PONG";
        }

        public async Task<string> RouteToStore(string[] data, CancellationToken cancellationToken)
        {
            if (await storeHandler.OnReceivedStore(new Contact(data[1]), new Tuple(data[2]), cancellationToken))
                return "OK";
            return "ERROR";
        }
    }
}

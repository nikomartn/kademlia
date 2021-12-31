using Router.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Router.Application
{
    public class CommandRouter
    {
        private readonly IHandlerProvider handlerProvider;

        public CommandRouter(IHandlerProvider handlerProvider)
        {
            this.handlerProvider = handlerProvider;
        }

        public Task<string> RouteRequest(string command, CancellationToken cancellationToken)
        {
            var data = command.Split(';');
            switch (data[0])
            {
                case "PING": return handlerProvider.RouteToPing(data, cancellationToken);
                case "FIND_NODE": return handlerProvider.RouteToFindNode(data, cancellationToken);
                case "FIND_VALUE": return handlerProvider.RouteToFindValue(data, cancellationToken);
                case "STORE": return handlerProvider.RouteToStore(data, cancellationToken);
                case "IDENTIFY": return handlerProvider.RouteToIdentify(data, cancellationToken);
                default: throw new UnrecognizedOperation();
            }
        }

        
    }
}

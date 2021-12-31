using Kademlia.Application.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Router.Domain
{
    public interface IHandlerProvider
    {
        Task<string> RouteToPing(string[] data, CancellationToken cancellationToken);
        Task<string> RouteToFindNode(string[] data, CancellationToken cancellationToken);
        Task<string> RouteToFindValue(string[] data, CancellationToken cancellationToken);
        Task<string> RouteToStore(string[] data, CancellationToken cancellationToken);
        Task<string> RouteToIdentify(string[] data, CancellationToken cancellationToken);
       
    }
}

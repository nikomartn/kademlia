using Autofac;
using Configuration;
using Kademlia.Application.Network;
using Kademlia.Application.User;
using Kademlia.Domain.Buckets;
using Kademlia.Domain.Clock;
using Kademlia.Domain.Clock.Contracts;
using Kademlia.Domain.Clock.Events;
using Kademlia.Domain.Database.Contracts;
using Kademlia.Domain.Interfaces.Client;
using Kademlia.Domain.Iteratives;
using Kademlia.Infraestructure.Client;
using Kademlia.Infraestructure.Database;
using Logger;
using Program.Controllers;
using Router.Application;
using Router.Domain;
using Router.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPLayer;

namespace Program.DependencyInyection
{
    public static class Builder
    {
        public static IContainer SyndicateDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<LinesUI.Router>().SingleInstance();
            builder.RegisterType<NotFoundController>().SingleInstance();
            builder.RegisterType<MenuController>().SingleInstance();
            builder.RegisterType<PublishController>().SingleInstance();
            builder.RegisterType<JoinNetworkController>().SingleInstance();
            builder.RegisterType<LogsController>().SingleInstance();
            builder.RegisterType<GetController>().SingleInstance();

            builder.RegisterType<Publish>();
            builder.RegisterType<JoinNetwork>();
            builder.RegisterType<Get>();

            builder.RegisterType<IterativeStore>();
            builder.RegisterType<IterativeFindNode>();
            builder.RegisterType<IterativeFindValue>();

            builder.RegisterType<TCPClient>().As<IClient>();

            builder.RegisterType<BucketContainer>().SingleInstance();

            builder.RegisterType<Configuration.Implementation.Configuration>().As<IConfiguration>().SingleInstance();

            builder.RegisterType<ClockManager>().As<IClockManager>().SingleInstance();
            builder.RegisterType<RepublishEvent>();
            builder.RegisterType<ReplicateEvent>();

            builder.RegisterType<InMemoryDataBase>().As<IDatabase>().SingleInstance();

            builder.RegisterType<TaskBasedTcpServer>().As<ITcpServer>();

            builder.RegisterType<CommandRouter>().SingleInstance();

            builder.RegisterType<HandlerProvider>().As<IHandlerProvider>().SingleInstance();

            builder.RegisterType<FindNodeHandler>().SingleInstance();
            builder.RegisterType<FindValueHandler>().SingleInstance();
            builder.RegisterType<IdentifyHandler>().SingleInstance();
            builder.RegisterType<PingHandler>().SingleInstance();
            builder.RegisterType<StoreHandler>().SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).InstancePerDependency();
            var loggerRepo = new LoggerRepo();
            builder.RegisterInstance(loggerRepo).As<ILoggerRepo>().SingleInstance();
            builder.RegisterInstance(loggerRepo).As<ILoggerForConsumer>().SingleInstance();


            return builder.Build();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Program.DependencyInyection;
using Autofac;
using Configuration;
using Program.Controllers;
using TCPLayer;
using Router.Application;
using Logger;

namespace Program
{
    public class MainClass
    {
        public static async Task Main(string[] args)
        {
            var container = Builder.SyndicateDependencies();
            var configuration = container.Resolve<IConfiguration>();
            configuration["tRepublish"] = "9999999";
            configuration["tRefresh"] = "9999999";
            configuration["tReplicate"] = "9999999";
            configuration["K"] = "20";
            configuration["Ip"] = args[0];
            configuration["Port"] = args[1];

            var router = container.Resolve<LinesUI.Router>();
            router.SyndicateControllerToUrl(container.Resolve<MenuController>(), "menu");
            router.SyndicateControllerToUrl(container.Resolve<PublishController>(), "publish");
            router.SyndicateControllerToUrl(container.Resolve<JoinNetworkController>(), "join");
            router.SyndicateControllerToUrl(container.Resolve<NotFoundController>(), "404");
            router.SyndicateControllerToUrl(container.Resolve<LogsController>(), "logs");
            router.SyndicateControllerToUrl(container.Resolve<GetController>(), "get");


            router.NavigateToUrl("menu");

            var server = new TaskBasedTcpServer(container.Resolve<CommandRouter>().RouteRequest, int.Parse(configuration["Port"]), Encoding.ASCII, configuration["Ip"], exceptionHandler: LogError);
            await server.StartListeningAsync(CancellationToken.None);
            
        }

        private static void LogError(Exception obj)
        {
            throw new NotImplementedException();
        }
    }
}

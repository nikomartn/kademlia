using LinesUI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinesUI
{
    public class Router
    {
        public Router()
        {
            ui.Start();
        }

        Dictionary<string, IController> controllers = new Dictionary<string, IController>();
        Ui ui = new Ui();

        public string ActualUrl { get; internal set; }

        public void SyndicateControllerToUrl(IController controller, string url)
        {
            controllers[url] = controller;
        }

        IController FetchOrThrowController(string url)
        {
            if (!controllers.Any((pair) => url.StartsWith(pair.Key)))
            {
                if (controllers.ContainsKey("404"))
                    return controllers["404"];
                else
                    throw new UnableToFindController();
            }

            return controllers.First((controller) => url.StartsWith(controller.Key)).Value;
        }

        public void NavigateToUrl(string url)
        {
            var controller = FetchOrThrowController(url);
            ui.Controller = controller;
            ActualUrl = url;
            controller.OnRoutedAsync();
        }
    }
}

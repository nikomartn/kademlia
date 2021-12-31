using LinesUI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinesUI;
namespace Program.Controllers
{
    public class MenuController : IController
    {
        public MenuController(LinesUI.Router router)
        {
            this.router = router;
        }

        Observable<string> text = new Observable<string>();
        private readonly LinesUI.Router router;

        public Observable<string> Text => text;

        public void UserCommandEventHandler(string command)
        {
            switch (command)
            {
                case "quit": Environment.Exit(0); break;
                default: router.NavigateToUrl(command.Replace(' ', '/')); break;
            }
            
        }

        public void OnRoutedAsync()
        {
            Text.Data = "KADEMLIA - Nicolás García Martín, Mario Alonso Nuñez @2021\n" +
                        "\n" +
                        "  Comandos ->\n" +
                        "    :publish <valor>\n" + 
                        "    :get <clave>\n " +
                        "    :join <ip> <port>\n" +
                        "    :logs\n" +
                        "    :quit";
        }
    }
}

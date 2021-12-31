using Kademlia.Application.User;
using LinesUI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Program.Controllers
{
    public class JoinNetworkController : IController
    {
        public JoinNetworkController(LinesUI.Router router, JoinNetwork joinNetwork)
        {
            this.router = router;
            this.joinNetwork = joinNetwork;
        }

        Observable<string> text = new Observable<string>();
        private readonly LinesUI.Router router;
        private readonly JoinNetwork joinNetwork;

        public Observable<string> Text => text;

        public async void OnRoutedAsync()
        {
            WriteLoading();
            await JoinNetworkAction();
            
        }

        private async Task JoinNetworkAction()
        {
            var ip = GetIpFromUrl();
            var port = GetPortFromUrl();
            if (ip == null || port == null)
            {
                Text.Data = "Error: no ha especificado un servidor de forma correcta: <ip> <port>";
                return;
            }
            try
            {
                var success = await joinNetwork.DoItAsync(ip, port, CancellationToken.None);
                if (success)
                    Text.Data = "Conectado.";
                else
                    Text.Data = "Incapaz de conectarse al nodo.";
            }
            catch (Exception e)
            {
                Text.Data = e.ToString();
            }
        }

        private void WriteLoading()
        {
            Text.Data = " @ Cargando..";
        }

        string GetIpFromUrl()
        {
            var data = router.ActualUrl.Split('/');
            if (data.Length < 3)
                return null;
            return data[1];
        }

        string GetPortFromUrl()
        {
            var data = router.ActualUrl.Split('/');
            if (data.Length < 3)
                return null;
            return data[2];
        }

        public void UserCommandEventHandler(string command)
        {
            router.NavigateToUrl("menu");
        }
    }
}

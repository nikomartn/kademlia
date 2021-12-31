using Kademlia.Application.User;
using Kademlia.Domain.Iteratives.Exception;
using LinesUI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Program.Controllers
{
    public class PublishController : IController
    {
        public PublishController(LinesUI.Router router, Publish publish)
        {
            this.router = router;
            this.publish = publish;
        }

        Observable<string> text = new Observable<string>();
        private readonly LinesUI.Router router;
        private readonly Publish publish;

        public Observable<string> Text => text;

        public async void OnRoutedAsync()
        {
            WriteLoading();
            await PublishData();
        }

        private void WriteLoading()
        {
            Text.Data = " @ Cargando..";
        }

        private async Task PublishData()
        {
            string data;
            string key;
            if( (data = GetDataFromUrl()) == null)
            {
                Text.Data = "No se ha dado nada a publicar.\n";
                return;
            }
            try
            {
                key = await publish.DoIt(data, CancellationToken.None);
                Text.Data = $"Publicado con éxito, la clave es: {key}";
            } catch (NotKnownNodesAvailable)
            {
                Text.Data = $"No estás conectado a una red.";
            } catch (Exception e)
            {
                Text.Data = $"Error al publicar: \n{e}";
            }
        }

        private string GetDataFromUrl()
        {
            var bodyParts = router.ActualUrl.Split('/');
            if (bodyParts.Length < 2)
                return null;

            return bodyParts[1];
        }

        public void UserCommandEventHandler(string command)
        {
            router.NavigateToUrl("menu");
        }
    }
}

using Kademlia.Application.User;
using LinesUI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tuple = Kademlia.Domain.Database.Contracts.Tuple;
namespace Program.Controllers
{
    public class GetController : IController
    {
        public GetController(LinesUI.Router router, Get get)
        {
            this.router = router;
            this.get = get;
        }

        Observable<string> text = new Observable<string>();
        private readonly LinesUI.Router router;
        private readonly Get get;

        public Observable<string> Text => text;

        public async void OnRoutedAsync()
        {
            
            WriteLoading();
            var key = GetDataFromUrl();
            if (key == null)
            {
                Text.Data = "No se ha pasado ninguna clave";
                return;
            }
            var value = await get.DoItAsync(key, CancellationToken.None);
            if (value == null)
            {
                Text.Data = $"No se ha encontrado valor para la clave:{key}";
                return;
            }
            WriteResult(value);
        }

        

        void WriteResult(Tuple tuple)
        {
            Text.Data = $"Clave:{tuple.Key}\nValor:{tuple.Value}";
        }

        private void WriteLoading()
        {
            Text.Data = " @ Cargando..";
        }

        private string GetDataFromUrl()
        {
            var data = router.ActualUrl.Split('/');
            if (data.Length < 2)
                return null;
            return data[1];
        }

        public void UserCommandEventHandler(string command)
        {
            router.NavigateToUrl("menu");
        }
    }
}

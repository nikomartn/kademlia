using LinesUI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.Controllers
{
    public class NotFoundController : IController
    {
        public NotFoundController(LinesUI.Router router)
        {
            this.router = router;
        }

        Observable<string> text = new Observable<string>();
        private readonly LinesUI.Router router;

        public Observable<string> Text => text;

        public void OnRoutedAsync()
        {
            text.Data = "Orden no entendida.";
        }

        public void UserCommandEventHandler(string command)
        {
            router.NavigateToUrl("menu");
        }
    }
}

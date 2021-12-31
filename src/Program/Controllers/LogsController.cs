using LinesUI.Domain;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.Controllers
{
    public class LogsController : IController
    {
        public LogsController(LinesUI.Router router, ILoggerForConsumer loggerForConsumer)
        {
            this.router = router;
            this.loggerForConsumer = loggerForConsumer;
            loggerForConsumer.GetLogs().OnTextChanged += LogsController_OnTextChanged;
        }

        private void LogsController_OnTextChanged()
        {
            ReevaluateText();
        }

        void ReevaluateText()
        {
            if (String.IsNullOrEmpty(loggerForConsumer.GetLogs().Data))
                text.Data = "No hay logs.";
            else
                text.Data = loggerForConsumer.GetLogs().Data;
        }

        private readonly LinesUI.Router router;
        private readonly ILoggerForConsumer loggerForConsumer;

        Observable<string> text = new Observable<string>();
        public Observable<string> Text => text;

        public void OnRoutedAsync()
        {
            ReevaluateText();
        }

        public void UserCommandEventHandler(string command)
        {
            router.NavigateToUrl("menu");
        }
    }
}

using LinesUI.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logger
{
    public interface ILoggerForConsumer
    {
        Observable<string> GetLogs();
    }
}

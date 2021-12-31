using LinesUI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logger
{
    public class LoggerRepo : ILoggerRepo, ILoggerForConsumer
    {
        Observable<string> observable = new Observable<string>();

        public Observable<string> GetLogs()
        {
            return observable;
        }

        public void Log(string v)
        {
            lock (observable)
            {
                observable.Data = v + '\n' + observable.Data;
            }
        }
    }
}

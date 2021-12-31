using System;

namespace Logger
{
    public class Logger<T> : ILogger<T>
    {
        T type;
        private readonly ILoggerRepo loggerForConsume;

        public Logger(ILoggerRepo loggerForConsume)
        {
            this.loggerForConsume = loggerForConsume;
        }

        public void LogError(string message)
        {
            loggerForConsume.Log($"[{DateTime.Now}] {type} ::ERROR:: {message}");
        }

        public void LogInfo(string message)
        {
            loggerForConsume.Log($"[{DateTime.Now}] {typeof(T)} ::Info:: {message}");
        }

        public void LogWarning(string message)
        {
            loggerForConsume.Log($"[{DateTime.Now}] {type} ::WARNING:: {message}");
        }
    }
}

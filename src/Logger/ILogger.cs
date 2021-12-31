using System;
using System.Collections.Generic;
using System.Text;

namespace Logger
{
    public interface ILogger<T>
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}

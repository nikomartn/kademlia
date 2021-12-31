using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kademlia.Domain.Clock.Contracts
{
    public interface IClockManager
    {
        void RemoveProgramming(object sender, string ticket);
        void Program(object sender,
                     string ticket,
                     Func<object, string, CancellationToken, Task> eventHandler,
                     int launchAfterSeconds,
                     Action<Exception> exceptionHandler = null);
        void ShutdownAllPrograms();
    }
}

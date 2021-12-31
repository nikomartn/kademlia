using Kademlia.Domain.Clock.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskExtensionsLib;

namespace Kademlia.Domain.Clock
{
    public class ClockManager : IClockManager
    {
        Dictionary<object, Dictionary<string, CancellationTokenSource>> programs = new Dictionary<object, Dictionary<string, CancellationTokenSource>>();

        public void Program(object sender, string ticket, Func<object, string, CancellationToken, Task> eventHandler, int launchAfterSeconds, Action<Exception> exceptionHandler = null)
        {
            programs[sender] = new Dictionary<string, CancellationTokenSource>();
            programs[sender][ticket] = new CancellationTokenSource();
            Programmed(eventHandler, ticket, launchAfterSeconds, programs[sender][ticket].Token).SafeFireAndForget(onException: exceptionHandler);
        }

        private async Task Programmed(Func<object, string, CancellationToken, Task> eventHandler, string ticket, int launchAfterSeconds, CancellationToken cancellationToken)
        {
            await Task.Delay(launchAfterSeconds, cancellationToken);
            await eventHandler?.Invoke(this, ticket, cancellationToken);
        }

        public void RemoveProgramming(object sender, string ticket)
        {
            if (programs.ContainsKey(sender))
                if (programs[sender].ContainsKey(ticket))
                {
                    programs[sender][ticket].Cancel();
                    programs[sender].Remove(ticket);
                }
        }

        public void ShutdownAllPrograms()
        {
            foreach (var subscriber in programs)
            {
                foreach (var program in subscriber.Value)
                {
                    program.Value.Cancel();
                    subscriber.Value.Remove(program.Key);
                }
                programs.Remove(subscriber.Key);
            }
        }
    }
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskExtensionsLib;
namespace TCPLayer
{
    /// <summary>
    /// TCPServer will listen to TCP requests and launch Tasks that will handle the bussines logic
    /// </summary>
    public class TaskBasedTcpServer : TcpUtils, ITcpServer
    {
        private readonly Func<string, CancellationToken, Task<string>> _callback;
        private readonly Action<Exception> _externalExceptionHandler;

        /// <summary>
        /// Intantiate TCPServer with a configuration.
        /// </summary>
        /// <param name="callback">Callback to invoke on a request, the result will be sent back.</param>
        /// <param name="port">Port to listen requests.</param>
        /// <param name="encoding">Encoding of the protocol.</param>
        /// <param name="ipAddress">IP addres to listen.</param>
        /// <param name="endOfFileToken">EOF token used after a stream of data.</param>
        /// <param name="exceptionHandler">Action to execute when an exception is thrown from a Task</param>
        public TaskBasedTcpServer(Func<string, CancellationToken, Task<string>> callback,
                         int port,
                         Encoding encoding,
                         string ipAddress = "0.0.0.0",
                         string endOfFileToken = "<EOF>",
                         int timeout = -1,
                         Action<Exception> exceptionHandler = null) : base(port, encoding, ipAddress, endOfFileToken, timeout)
        {
            _callback = callback;
            _externalExceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Start the listening server.
        /// </summary>
        /// <param name="cancellationToken">Cancelation token to stop this server and all Tasks created for requests.</param>
        /// <returns></returns>
        public async Task StartListeningAsync(CancellationToken cancellationToken)
        {
            var listener = new TcpListener(IPAddress.Parse(_ipAddress), _port);
            using (cancellationToken.Register(listener.Stop))
            {
                try
                {
                    listener.Start();
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        TcpClient client = await listener.AcceptTcpClientAsync();
                        HandleRequestAsync(client, cancellationToken).SafeFireAndForget(onException: _externalExceptionHandler);
                    }
                }
                catch (Exception e)
                {
                    if (cancellationToken.IsCancellationRequested) return;
                    throw e;
                }
            }
        }

        /// <summary>
        /// Handle a request with it's client.
        /// </summary>
        /// <param name="client">Client representing the accepted connection.</param>
        /// <param name="cancellationToken">For cancelling operations.</param>
        /// <returns></returns>
        protected async Task HandleRequestAsync(TcpClient client, CancellationToken cancellationToken)
        {
            string received = await ReadUntilEof(client.GetStream(), cancellationToken);
            string response = await _callback.Invoke(received, cancellationToken);
            await WriteUntilEof(response, client.GetStream(), cancellationToken);
            client.GetStream().Close();
            client.Close();
        }
    }
}

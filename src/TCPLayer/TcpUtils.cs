using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPLayer
{
    public class TcpUtils
    {
        protected readonly int _port;
        protected readonly string _ipAddress;
        protected readonly Encoding _encoding;
        protected readonly string _endOfFileToken;
        private readonly int _timeout;

        /// <summary>
        /// Intantiate A TcpHandler with a configuration.
        /// </summary>
        /// <param name="callback">Callback to invoke on a request, the result will be sent back.</param>
        /// <param name="port">Port to listen requests.</param>
        /// <param name="encoding">Encoding of the protocol.</param>
        /// <param name="ipAddress">IP addres to listen.</param>
        /// <param name="endOfFileToken">EOF token used after a stream of data.</param>
        /// <param name="exceptionHandler">Action to execute when an exception is thrown from a Task</param>
        public TcpUtils(int port,
                         Encoding encoding,
                         string ipAddress,
                         string endOfFileToken = "<EOF>",
                         int timeout = -1)
        {
            _port = port;
            _ipAddress = ipAddress;
            _encoding = encoding;
            _endOfFileToken = endOfFileToken;
            _timeout = timeout;
        }

        /// <summary>
        /// Read string from stream until EOF is found or cancellation.
        /// </summary>
        /// <param name="stream">Stream where data will be read.</param>
        /// <param name="cancellationToken">Token for cancellation.</param>
        /// <returns></returns>
        protected async Task<string> ReadUntilEof(NetworkStream stream, CancellationToken cancellationToken)
        {
            stream.ReadTimeout = _timeout;
            string content = string.Empty;
            byte[] buffer = new byte[256];
            while (content.IndexOf(_endOfFileToken) < 0)
            {
                int bytesRead = await stream.ReadAsync(buffer, cancellationToken);
                if (bytesRead > 0)
                    content += _encoding.GetString(buffer);
            }
            content = content.Remove(content.IndexOf("<EOF>"));
            return content;
        }

        /// <summary>
        /// Write string over stream until EOF or cancellation.
        /// </summary>
        /// <param name="response">String to write.</param>
        /// <param name="networkStream">Stream to write to.</param>
        /// <param name="cancellationToken">Token for cancellation.</param>
        /// <returns></returns>
        protected async Task WriteUntilEof(string response, NetworkStream networkStream, CancellationToken cancellationToken)
        {
            networkStream.WriteTimeout = _timeout;
            var buffer = _encoding.GetBytes(response + _endOfFileToken);
            await networkStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
        }


    }
}

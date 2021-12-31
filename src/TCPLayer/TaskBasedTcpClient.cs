using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPLayer
{
    /// <summary>
    /// TcpClient that allows communication with a server, implemented with Task
    /// </summary>
    public class TaskBasedTcpClient : TcpUtils, ITcpClient
    {
        /// <summary>
        /// Instantiate TaskBasedTcpClient
        /// </summary>
        /// <param name="ipAddress">Address of the server.</param>
        /// <param name="port">Port of service.</param>
        /// <param name="encoding">Encoding.</param>
        /// <param name="timeout">Timeout.</param>
        /// <param name="endOfFileToken">EndOfFileToken</param>
        public TaskBasedTcpClient(string ipAddress,
                         int port,
                         Encoding encoding,
                         int timeout,
                         string endOfFileToken = "<EOF>") : base(port, encoding, ipAddress, endOfFileToken, timeout)
        {

        }

        /// <summary>
        /// Start communication with server, return the response.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> Communicate(string message, CancellationToken cancellationToken)
        {
            using (var client = new System.Net.Sockets.TcpClient(_ipAddress, _port))
            {
                await WriteUntilEof(message, client.GetStream(), cancellationToken);
                return await ReadUntilEof(client.GetStream(), cancellationToken);
            }
        }
    }
}

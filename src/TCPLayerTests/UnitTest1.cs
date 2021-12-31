using NUnit.Framework;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCPLayer;

namespace TCPLayerTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        static Task<string> ToUpper(string d, CancellationToken ct)
        {
            return Task.FromResult(d.ToUpper());
        }

        [Test]
        public async Task ClientAndServerCanCommunicate()
        {
            var server = new TaskBasedTcpServer(ToUpper, 11000, Encoding.ASCII, timeout: 5000);
            var client = new TaskBasedTcpClient("127.0.0.1", 11000, Encoding.ASCII, timeout: 5000);
            CancellationTokenSource cts = new CancellationTokenSource();
            var s = server.StartListeningAsync(cts.Token);
            //Thread.Sleep(1000);
            var c = client.Communicate("hello world", CancellationToken.None).Result;
            Assert.AreEqual("HELLO WORLD", c);
            //Thread.Sleep(1000);
            cts.Cancel();
            await s;
        }

    }
}
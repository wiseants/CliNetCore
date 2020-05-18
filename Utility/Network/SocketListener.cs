// https://github.com/Astn/JSON-RPC.NET/wiki/Getting-Started-(Sockets)  인용.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utility.Network
{
    /// <summary>
    /// 비동기 소케 리스너.
    /// </summary>
    public class SocketListener : TcpListener
    {
        #region Constructors

        [Obsolete]
        public SocketListener(int port) : base(port)
        {

        }

        public SocketListener(IPEndPoint localEP) : base(localEP)
        {

        }

        public SocketListener(IPAddress localaddr, int port) : base(localaddr, port)
        {

        }

        #endregion

        #region Public methods

        public async void StartAsync(int listenPort, Action<StreamWriter, string> handleRequest, CancellationToken token)
        {
            Start();

            token.Register(state =>
            {
                ((TcpListener)state).Stop();
            }, this);

            while (token.IsCancellationRequested == false)
            {
                try
                {
                    TcpClient tc = await AcceptTcpClientAsync().ConfigureAwait(false);
                    _ = Task.Factory.StartNew(o => AsyncTcpProcess(o, handleRequest), tc, token);
                }
                catch (Exception) when (token.IsCancellationRequested == true)
                {
                }
            }
        }

        #endregion

        #region Private methods

        private void AsyncTcpProcess(object o, Action<StreamWriter, string> handleRequest)
        {
            if (o is TcpClient == false)
            {
                return;
            }

            using (var stream = ((TcpClient)o).GetStream())
            {
                var reader = new StreamReader(stream, Encoding.UTF8);
                while (reader.EndOfStream == false)
                {
                    var receivedMessage = reader.ReadLine();
                    handleRequest(new StreamWriter(stream, new UTF8Encoding(false)), receivedMessage);
                }
            }
        }

        #endregion
    }
}

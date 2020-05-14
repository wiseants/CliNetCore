// https://github.com/Astn/JSON-RPC.NET/wiki/Getting-Started-(Sockets)  인용.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Network
{
    public class SocketListener
    {
        private TcpListener listener = null;
        public async Task StartAsync(int listenPort, Action<StreamWriter, string> handleRequest)
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), listenPort);
            listener.Start();

            while (true)
            {
                try
                {
                    TcpClient tc = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    await Task.Factory.StartNew(o => AsyncTcpProcess(o, handleRequest), tc);
                }
                catch (Exception e)
                {
                    Console.WriteLine("RPCServer exception " + e);
                }
            }
        }

        void AsyncTcpProcess(object o, Action<StreamWriter, string> handleRequest)
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
    }
}

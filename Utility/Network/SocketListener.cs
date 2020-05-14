// https://github.com/Astn/JSON-RPC.NET/wiki/Getting-Started-(Sockets)  인용.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Utility.Network
{
    public class SocketListener
    {
        private TcpListener listener = null;
        public void Start(int listenPort, Action<StreamWriter, string> handleRequest)
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), listenPort);
            listener.Start();

            while (true)
            {
                try
                {
                    //Task<TcpClient> clientTask = server.AcceptTcpClientAsync();
                    //clientTask.Wait()
                    using (var client = listener.AcceptTcpClient())
                    using (var stream = client.GetStream())
                    {
                        var reader = new StreamReader(stream, Encoding.UTF8);
                        while (reader.EndOfStream == false)
                        {
                            var receivedMessage = reader.ReadLine();
                            handleRequest(new StreamWriter(stream, new UTF8Encoding(false)), receivedMessage);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("RPCServer exception " + e);
                }
            }
        }
    }
}

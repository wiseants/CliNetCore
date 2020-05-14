// https://github.com/Astn/JSON-RPC.NET/wiki/Getting-Started-(Sockets)

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Network
{
    public class SocketListener
    {
        public static void Start(int listenPort, Action<StreamWriter, string> handleRequest)
        {
            var server = new TcpListener(IPAddress.Parse("127.0.0.1"), listenPort);
            server.Start();

            while (true)
            {
                try
                {
                    //Task<TcpClient> clientTask = server.AcceptTcpClientAsync();
                    //clientTask.Wait()
                    using (var client = server.AcceptTcpClient())
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

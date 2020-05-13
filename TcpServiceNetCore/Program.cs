using AustinHarris.JsonRpc;
using System;
using System.IO;
using TcpServiceNetCore.Services;
using Utility.Network;

namespace TcpServiceNetCore
{
    public class Program
    {
        private static object _svc;
        static void Main(string[] args)
        {
            // must new up an instance of the service so it can be registered to handle requests.
            _svc = new ExampleService();

            var rpcResultHandler = new AsyncCallback(
                state =>
                {
                    var async = ((JsonRpcStateAsync)state);
                    var result = async.Result;
                    var writer = ((StreamWriter)async.AsyncState);

                    writer.WriteLine(result);
                    writer.FlushAsync();
                });

            SocketListener.Start(3333, (writer, line) =>
            {
                var async = new JsonRpcStateAsync(rpcResultHandler, writer) { JsonRpc = line };
                JsonRpcProcessor.Process(async, writer);
            });
        }
    }
}

using AustinHarris.JsonRpc;
using CliNetCore.Interfaces;
using CommandLine;
using System;
using System.Net;
using System.Text;
using Utility.Network;

namespace CliNetCore.Cores.Commands
{
    [Verb("decrease", HelpText = "Decrease number")]
    public class DecreaseCommand : IAction
    {
        public bool IsValid => true;

        [Option('t', "target", Required = true, HelpText = "target number.")]
        public int TargetNumber
        {
            get;
            set;
        }

        public int Action()
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8055);

            JsonRpcClient client = new JsonRpcClient(point, Encoding.UTF8);
            var result = client.Invoke<int>("Decr", new object[] { TargetNumber });
            result.Subscribe((JsonResponse<int> response) =>
            {
                Console.WriteLine(response.Result);
            });

            return 1;
        }
    }
}

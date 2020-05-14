using AustinHarris.JsonRpc;
using CliNetCore.Interfaces;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Utility.Network;

namespace CliNetCore.Cores.Commands
{
    [Verb("increase", HelpText = "Increase number")]
    public class AddCommand : IAction
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
            IPEndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3333);

            JsonRpcClient client = new JsonRpcClient(point, Encoding.UTF8);
            var result = client.Invoke<int>("Incr", new object[] { TargetNumber });
            result.Subscribe((JsonResponse<int> response) =>
            {
                Console.WriteLine(response.Result);
            });

            return 1;
        }
    }
}

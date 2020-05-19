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
    [Verb("appraisal", HelpText = "Appraisal of analysis results.")]
    public class AppraisalCommand : IAction
    {
        public bool IsValid => true;

        [Option('i', "ip", Required = true, HelpText = "IP address")]
        public string Ip
        {
            get;
            set;
        }

        [Option('p', "port", Required = true, HelpText = "Port number")]
        public int Port
        {
            get;
            set;
        }

        [Option('n', "project", Required = true, HelpText = "Project name")]
        public string ProjectName
        {
            get;
            set;
        }

        [Option('u', "user", Required = true, HelpText = "User ID")]
        public string UserId
        {
            get;
            set;
        }

        [Option('h', "high", Required = true, HelpText = "High risk count")]
        public int HighRiskCount
        {
            get;
            set;
        }

        [Option('m', "middle", Required = true, HelpText = "Middle risk count")]
        public int MiddleRiskCount
        {
            get;
            set;
        }

        [Option('r', "row", Required = true, HelpText = "Row risk count")]
        public int RowRiskCount
        {
            get;
            set;
        }

        public int Action()
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(Ip), Port);

            JsonRpcClient client = new JsonRpcClient(point, Encoding.UTF8);
            var result = client.Invoke<int>("vulnerability.GetSatisfactionResult", new object[] 
            { 
                ProjectName, 
                UserId, 
                HighRiskCount, 
                MiddleRiskCount, 
                RowRiskCount 
            });

            result.Subscribe((JsonResponse<int> response) =>
            {
                Console.WriteLine(response.Result);
            });

            return 1;
        }
    }
}

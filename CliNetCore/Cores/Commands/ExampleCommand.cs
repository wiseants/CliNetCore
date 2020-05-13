using CommandLine;
using CliNetCore.Interfaces;
using System;
using System.Threading.Tasks;
using AustinHarris.JsonRpc;
using System.Net.Sockets;
using System.Net;
using Utility.Network;
using System.Text;

namespace CliNetCore.Cores.Commands
{
    /// <summary>
    /// 명령어 예제.
    /// </summary>
    [Verb("example", HelpText = "Command example.")]
    public class ExampleCommand : IAction
    {
        /// <summary>
        /// 유효성.
        /// </summary>
        public bool IsValid => true;

        /// <summary>
        /// 필수 옵션.
        /// </summary>
        [Option('r', "required", Required = false, HelpText = "is a required parameter.")]
        public string Ip
        {
            get;
            set;
        }

        /// <summary>
        /// 선택 옵션.
        /// </summary>
        [Option('o', "option", Required = false, HelpText = "is a optional parameter.")]
        public int Port
        {
            get;
            set;
        }

        /// <summary>
        /// 명령 실행 메소드.
        /// </summary>
        /// <returns>-1:종료, 그 외는 처리 결과.</returns>
        public int Action()
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5050);

            //var sessionid = Handler.DefaultSessionId();
            //Task<string> task = JsonRpcProcessor.Process(sessionid, "{'method':'add','params':[1,2],'id':1}", this);
            //task.Wait(1000);

            //JsonRpcClient client = new JsonRpcClient(point, Encoding.UTF8);
            //client.InvokeSingleArgument

            Console.WriteLine("Executed example command. Required:{0}, Option:{1}", Ip, Port);
            return 1;
        }
    }
}

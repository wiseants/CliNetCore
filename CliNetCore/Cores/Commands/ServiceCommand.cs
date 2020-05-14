using CliNetCore.Interfaces;
using CommandLine;
using System;
using Utility.Network;

namespace CliNetCore.Cores.Commands
{
    [Verb("service", HelpText = "Use service.")]
    public class ServiceCommand : IAction
    {
        #region Properties

        public bool IsValid => true;

        [Option('p', "power", Required = true, HelpText = "Power on/off service.")]
        public int PowerOnOff { get; set; }

        #endregion

        #region Public methods

        public int Action()
        {
            if (PowerOnOff == 1)
            {
                Console.WriteLine("Start service.");
                RpcServiceManager.Instance.Start();
            }
            else
            {
                Console.WriteLine("Stop service.");
                RpcServiceManager.Instance.Stop();
            }

            return 1;
        }

        #endregion
    }
}

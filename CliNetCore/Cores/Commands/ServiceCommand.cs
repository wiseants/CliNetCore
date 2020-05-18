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

        [Option('p', "power", Required = true, HelpText = "turn on/turn off service.")]
        public string Power { get; set; }

        private bool IsPowerOn
        {
            get
            {
                return Power.Equals("on", StringComparison.OrdinalIgnoreCase) || Power.Equals("off", StringComparison.OrdinalIgnoreCase);
            }
        }

        #endregion

        #region Public methods

        public int Action()
        {
            bool result = false;
            if (IsPowerOn == false)
            {
                Console.WriteLine("Required parameter 'on' or 'off'");
                return result == true ? 1 : 0;
            }

            if (Power.Equals("on", StringComparison.OrdinalIgnoreCase) == true)
            {
                RpcServiceManager.Instance.Start();
            }
            else if (Power.Equals("off", StringComparison.OrdinalIgnoreCase) == true)
            {
                RpcServiceManager.Instance.Stop();
            }

            return 1;
        }

        #endregion
    }
}

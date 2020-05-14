using AustinHarris.JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using TcpServiceNetCore.Services;
using Utility.Network;

namespace CliNetCore.Cores
{
    /// <summary>
    /// RPC 서비스 싱글톤 클래스.
    /// </summary>
    public class ServiceManager : JsonRpcServer
    {
        #region Fields

        private static ServiceManager _instance = null;
        private static readonly object lockObject = new object();

        #endregion

        #region Properties

        /// <summary>
        /// 싱글톤 인스턴스.
        /// </summary>
        public static ServiceManager Instance
        {
            get 
            {
                if (_instance == null)
                {
                    lock(lockObject)
                    {
                        _instance = new ServiceManager
                        {
                            Services = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(s => s.GetTypes())
                            .Where(p => typeof(JsonRpcService).IsAssignableFrom(p) && p.IsAbstract == false)
                            .Select(type => (JsonRpcService)Activator.CreateInstance(type)).ToList()
                        };
                    }
                }

                return _instance;
            }
        }

        #endregion
    }
}

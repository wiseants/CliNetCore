using AustinHarris.JsonRpc;
using System;
using System.Linq;
using Utility.Network;

namespace CliNetCore.Cores
{
    /// <summary>
    /// RPC 서비스 싱글톤 클래스.
    /// </summary>
    public class RpcServiceManager : JsonRpcServer
    {
        #region Fields

        private static RpcServiceManager _instance = null;
        private static readonly object lockObject = new object();

        #endregion

        #region Properties

        /// <summary>
        /// 싱글톤 인스턴스.
        /// </summary>
        public static RpcServiceManager Instance
        {
            get 
            {
                if (_instance == null)
                {
                    lock(lockObject)
                    {
                        _instance = new RpcServiceManager
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

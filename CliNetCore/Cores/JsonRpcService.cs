using System;
using System.Collections.Generic;
using System.Text;
using Utility.Network;

namespace CliNetCore.Cores
{
    public class JsonRpcService : JsonRpcServer
    {
        private static JsonRpcService _instance = null;

        #region Properties

        public static JsonRpcService Instance
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new JsonRpcService();
                }

                return _instance;
            }
        }

        #endregion
    }
}

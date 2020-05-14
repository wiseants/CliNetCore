﻿using AustinHarris.JsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utility.Network
{
    /// <summary>
    ///  Json 포멧을 이용한 RPC 서버 클래스.
    /// </summary>
    public class JsonRpcServer
    {
        #region Fields

        private readonly List<object> serviceArray = new List<object>();

        #endregion

        #region Properties

        public int Port { get; set; } = 8055;

        public ICollection<JsonRpcService> Services { get; set; } = new List<JsonRpcService>();

        #endregion

        #region Public methods

        public void Start()
        {
            var rpcResultHandler = new AsyncCallback(state =>
            {
                using(var writer = ((StreamWriter)state.AsyncState))
                {
                    writer.WriteLine(((JsonRpcStateAsync)state).Result);
                    writer.FlushAsync();
                }
            });

            SocketListener.Start(Port, (writer, line) =>
            {
                var async = new JsonRpcStateAsync(rpcResultHandler, writer) 
                { 
                    JsonRpc = line 
                };

                JsonRpcProcessor.Process(async, writer);
            });
        }

        #endregion
    }
}
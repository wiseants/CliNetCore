﻿// https://github.com/Astn/JSON-RPC.NET/wiki/Getting-Started-(Sockets) 인용.

using AustinHarris.JsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Utility.Network
{
    /// <summary>
    ///  Json 포멧을 이용한 RPC 서버 클래스.
    /// </summary>
    public class JsonRpcServer
    {
        #region Fields

        private readonly List<object> serviceArray = new List<object>();
        private SocketListener socketListener = null;
        private CancellationTokenSource tokenSource = null;

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
                var async = ((JsonRpcStateAsync)state);
                var result = async.Result;
                var writer = ((StreamWriter)async.AsyncState);

                writer.WriteLine(result);
                writer.FlushAsync();
            });

            socketListener = new SocketListener();
            tokenSource = new CancellationTokenSource();

            socketListener.StartAsync(Port, (writer, line) =>
            {
                var async = new JsonRpcStateAsync(rpcResultHandler, writer)
                {
                    JsonRpc = line
                };

                JsonRpcProcessor.Process(async, writer);
            }, tokenSource.Token);
        }

        public void Stop()
        {
            tokenSource.Cancel();
        }

        #endregion
    }
}
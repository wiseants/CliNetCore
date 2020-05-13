using AustinHarris.JsonRpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliNetCore.Cores.Services
{
    public class TestService : JsonRpcService
    {
        [JsonRpcMethod]
        private int Add(int l, int r)
        {
            return l + r;
        }
    }
}

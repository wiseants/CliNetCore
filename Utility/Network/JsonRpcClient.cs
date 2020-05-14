// https://stackoverflow.com/questions/992456/sample-code-for-json-rpc-client-in-c-sharp

using AustinHarris.JsonRpc;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;

namespace Utility.Network
{
    /// <summary>
    /// Json 포멧을 이용한 RPC 클라이언트 클래스.
    /// </summary>
    public class JsonRpcClient
    {
        #region Fields

        private static readonly object idLock = new object();
        private static int id = 0;

        #endregion

        #region Properties

        public Encoding Encoding { get; set; }

        public IPEndPoint ServiceEndPoint { get; set; }

        #endregion

        #region Constructors

        public JsonRpcClient(IPEndPoint serviceEndpoint, Encoding encoding)
        {
            this.ServiceEndPoint = serviceEndpoint;
            this.Encoding = encoding;
        }

        #endregion

        #region Public methods

        public IObservable<JsonResponse<T>> InvokeWithScheduler<T>(string method, object arg, IScheduler scheduler)
        {
            var req = new AustinHarris.JsonRpc.JsonRequest()
            {
                Method = method,
                Params = new object[] { arg }
            };
            return InvokeRequestWithScheduler<T>(req, scheduler);
        }

        public IObservable<JsonResponse<T>> InvokeSingleArgument<T>(string method, object arg)
        {
            var req = new AustinHarris.JsonRpc.JsonRequest()
            {
                Method = method,
                Params = new object[] { arg }
            };
            return InvokeRequest<T>(req);
        }

        public IObservable<JsonResponse<T>> InvokeWithScheduler<T>(string method, object[] args, IScheduler scheduler)
        {
            var req = new AustinHarris.JsonRpc.JsonRequest()
            {
                Method = method,
                Params = args
            };
            return InvokeRequestWithScheduler<T>(req, scheduler);
        }

        public IObservable<JsonResponse<T>> Invoke<T>(string method, object[] args)
        {
            var req = new AustinHarris.JsonRpc.JsonRequest()
            {
                Method = method,
                Params = args
            };
            return InvokeRequest<T>(req);
        }

        public IObservable<JsonResponse<T>> InvokeRequestWithScheduler<T>(JsonRequest jsonRpc, IScheduler scheduler)
        {
            var res = Observable.Create<JsonResponse<T>>((obs) =>
                scheduler.Schedule(() => {

                    MakeRequest<T>(jsonRpc, obs);
                }));

            return res;
        }

        public IObservable<JsonResponse<T>> InvokeRequest<T>(JsonRequest jsonRpc)
        {
            return InvokeRequestWithScheduler<T>(jsonRpc, ImmediateScheduler.Instance);
        }

        #endregion

        #region Private methods

        private static Stream CopyAndClose(Stream inputStream)
        {
            const int readSize = 256;
            byte[] buffer = new byte[readSize];
            MemoryStream ms = new MemoryStream();

            int count = inputStream.Read(buffer, 0, readSize);
            while (count > 0)
            {
                ms.Write(buffer, 0, count);
                count = inputStream.Read(buffer, 0, readSize);
            }
            ms.Position = 0;
            inputStream.Close();
            return ms;
        }

        private string SendAndReceive(string messageToSend)
        {
            string res = null;

            // Data buffer for incoming data.
            byte[] bytes = new byte[1024];

            // Connect to a remote device.
            try
            {
                // Create a TCP/IP  socket.
                Socket socket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    socket.Connect(this.ServiceEndPoint);

                    Console.WriteLine("Socket connected to " + socket.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.GetBytes(messageToSend);

                    // Send the data through the socket.
                    int bytesSent = socket.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = socket.Receive(bytes);
                    res = Encoding.GetString(bytes, 0, bytesRec);
                    Console.Write("Server response = " + res);

                    // Release the socket.
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.Write("ArgumentNullException : " + ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.Write("SocketException : " + se.ToString());
                }
                catch (Exception e)
                {
                    Console.Write("Unexpected exception : " + e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            return res;
        }

        private void MakeRequest<T>(JsonRequest jsonRpc, IObserver<JsonResponse<T>> obs)
        {
            JsonResponse<T> rjson = null;
            string sstream = "";
            try
            {
                int myId;
                lock (idLock)
                {
                    myId = ++id;
                }
                jsonRpc.Id = myId.ToString();
            }
            catch (Exception ex)
            {
                obs.OnError(ex);
            }

            try
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonRpc) + "\r\n";
                if (typeof(T).Equals(typeof(Nil)))
                {
                    SendAndReceive(json);
                    rjson = new JsonResponse<T>();
                }
                else
                {
                    sstream = SendAndReceive(json);
                    rjson = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResponse<T>>(sstream);
                }
            }
            catch (Exception ex)
            {
                obs.OnError(ex);
            }

            if (rjson == null)
            {
                string exceptionMessage = "";
                try
                {
                    JObject jo = Newtonsoft.Json.JsonConvert.DeserializeObject(sstream) as JObject;
                    exceptionMessage = jo["Error"].ToString();
                }
                catch (Exception ex)
                {
                    exceptionMessage = sstream + "\r\n" + ex.Message;
                }
                obs.OnError(new Exception(exceptionMessage));
            }
            else
            {
                obs.OnNext(rjson);
            }

            obs.OnCompleted();
        }

        #endregion
    }
}

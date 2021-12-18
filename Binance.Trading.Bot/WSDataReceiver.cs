using Binance.Trading.Bot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Binance.Trading.Bot
{
    public class WSDataReceiver
    {
        private readonly ClientWebSocket socket;
        private readonly List<string> SubscribeRequestParams;

        public delegate void HandleReceivedData(string data);
        public event HandleReceivedData handleDataFunc;

        public WSDataReceiver()
        {
            socket = new();
            SubscribeRequestParams = new();
        }

        public async Task StartReceiver()
        {
            if (socket.State == WebSocketState.Open)
                return;

            await socket.ConnectAsync(new Uri("wss://stream.binance.com:9443/ws"), CancellationToken.None);
            await Subscribe();

            Thread readThread = new Thread(
              async delegate (object obj)
              {
                  byte[] recBytes = new byte[1024];
                  while (true)
                  {
                      var arraySegment = new ArraySegment<byte>(recBytes);
                      var receiveAsync = await socket.ReceiveAsync(arraySegment, CancellationToken.None);
                      string jsonString = Encoding.UTF8.GetString(recBytes);
                      handleDataFunc(jsonString);
                      recBytes = new byte[1024];
                  }

              });

            readThread.Start();
        }
        private async Task Subscribe()
        {
            var request = new SubscribeRequest{Id = 1,Params = SubscribeRequestParams};
            string js = JsonConvert.SerializeObject(request);
            byte[] bytes = Encoding.UTF8.GetBytes(js);
            var arraySegment = new ArraySegment<byte>(bytes);
            await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public WSDataReceiver SubscribeKline(params string[] sysmbols) => AddSubscribeParams(sysmbols, "@kline_1m");
        public WSDataReceiver SubscribeAggTrade(params string[] sysmbols) => AddSubscribeParams(sysmbols, "@aggTrade");
        private WSDataReceiver AddSubscribeParams(string[] sysmbols, string paramType)
        {
            SubscribeRequestParams.AddRange(
               sysmbols.Where(s => !SubscribeRequestParams.Any(x => x == string.Concat(s, paramType)))
                       .Select(s => string.Concat(s, paramType))
               );
            return this;
        }
    }
}

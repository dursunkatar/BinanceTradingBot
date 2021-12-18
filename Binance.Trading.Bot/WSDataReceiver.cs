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
        private readonly List<string> subscribeRequestParams;
        private readonly Dictionary<string, HandleReceivedData> handlers;

        public delegate void HandleReceivedData(string data);

        public WSDataReceiver()
        {
            socket = new();
            handlers = new();
            subscribeRequestParams = new();
        }

        public async Task StartReceiver()
        {
            if (socket.State == WebSocketState.Open)
                return;

            await socket.ConnectAsync(new Uri("wss://stream.binance.com:9443/ws"), CancellationToken.None);
            await subscribe();
            await ReceiveData();
        }

        private Task ReceiveData()
        {
            Task task = new Task(async () =>
           {
               byte[] recBytes = new byte[2048];
               while (true)
               {
                   string jsonString = await Utility.GetWSStreamReceivedData(socket);

                   //Task handleTask = new Task(() => handleDataFunc(jsonString));
                   //handleTask.Start();
               }
           });
            task.Start();
            return task;
        }
        private async Task subscribe()
        {
            var param = Utility.BuildWSStreamParam(subscribeRequestParams);
            await socket.SendAsync(param, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public WSDataReceiver SubscribeKline(HandleReceivedData handleFunc, params string[] sysmbols)
        {
            addHandle(StreamEventTypes.KLINE, handleFunc);
            addSubscribeParams(sysmbols, "@kline_1m");
            return this;
        }
        public WSDataReceiver SubscribeAggTrade(HandleReceivedData handleFunc, params string[] sysmbols)
        {
            addHandle(StreamEventTypes.AGG_TRADE, handleFunc);
            addSubscribeParams(sysmbols, "@aggTrade");
            return this;
        }
        private void addSubscribeParams(string[] sysmbols, string paramType)
        {
            subscribeRequestParams.AddRange(
               sysmbols.Where(s => !subscribeRequestParams.Any(x => x == string.Concat(s, paramType)))
                       .Select(s => string.Concat(s, paramType))
               );
        }
        private void addHandle(string eventType, HandleReceivedData handleFunc)
        {
            if (!handlers.ContainsKey(eventType))
                handlers.Add(eventType, handleFunc);
        }
    }
}

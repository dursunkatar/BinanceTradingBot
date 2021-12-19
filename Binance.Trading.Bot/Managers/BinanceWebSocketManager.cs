using Binance.Trading.Bot.Helpers;
using Binance.Trading.Bot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Managers
{
    public class BinanceWebSocketManager
    {
        private readonly ClientWebSocket socket;
        private readonly List<string> subscribeRequestParams;
        private readonly string baseUrl = "wss://stream.binance.com:9443/ws";
        private HandleReceivedData<Kline> OnKlineDataReceived;
        private HandleReceivedData<AggTrade> OnAggTradeDataReceived;
        public delegate void HandleReceivedData<TResponse>(TResponse data);
        public BinanceWebSocketManager()
        {
            socket = new();
            subscribeRequestParams = new();
        }
        public async Task StartReceiver()
        {
            if (socket.State == WebSocketState.Open)
                return;

            await socket.ConnectAsync(new Uri(baseUrl), CancellationToken.None);
            Console.WriteLine(socket.State);
            await subscribe();
            await receiveData();
        }
        private Task subscribe()
        {
            var param = WSHelper.BuildWSStreamParam(subscribeRequestParams);
            return socket.SendAsync(param, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        private Task receiveData()
        {
            Task task = new Task(async () =>
            {
                for (; ; )
                {
                    var (eventType, data) = await WSHelper.GetWSStreamReceivedDataAndEventType(socket);
                    Task t = new Task(() => callHandleFunc(eventType, data));
                    t.Start();
                }
            });
            task.Start();
            return task;
        }
        private void callHandleFunc(string eventType, string data)
        {
            if (eventType == StreamEventTypes.KLINE && OnKlineDataReceived != null)
            {
                Kline kline = JsonConvert.DeserializeObject<Kline>(data);
                if (kline.Candle.IsClosed)
                {
                    kline.Candle.Timestamp = DateTimeHelper.UnixTimestampToDateTime(double.Parse(kline.Candle.UnixTimestamp));
                    OnKlineDataReceived(kline);
                }
            }
            else if (eventType == StreamEventTypes.AGG_TRADE && OnAggTradeDataReceived != null)
            {
                AggTrade aggTrade = JsonConvert.DeserializeObject<AggTrade>(data);
                OnAggTradeDataReceived(aggTrade);
            }
        }
        private void addSubscribeParams(string[] sysmbols, string paramType)
        {
            subscribeRequestParams.AddRange(
               sysmbols.Where(s => !subscribeRequestParams.Any(x => x == string.Concat(s.Replace("I", "i").ToLower(), paramType)))
                       .Select(s => string.Concat(s.Replace("I", "i").ToLower(), paramType))
               );
        }
        public BinanceWebSocketManager SubscribeKline(HandleReceivedData<Kline> onKlineDataReceived, params string[] sysmbols)
        {
            OnKlineDataReceived = onKlineDataReceived;
            addSubscribeParams(sysmbols, "@kline_3m");
            return this;
        }
        public BinanceWebSocketManager SubscribeAggTrade(HandleReceivedData<AggTrade> onAggTradeDataReceived, params string[] sysmbols)
        {
            OnAggTradeDataReceived = onAggTradeDataReceived;
            addSubscribeParams(sysmbols, "@aggTrade");
            return this;
        }
    }
}

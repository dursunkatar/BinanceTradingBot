﻿using Binance.Trading.Bot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using static Binance.Trading.Bot.WSDataReceiver;

namespace Binance.Trading.Bot
{
    public class WSDataReceiver
    {
        private readonly ClientWebSocket socket;
        private readonly List<string> subscribeRequestParams;
        private HandleReceivedData<Kline> OnKlineDataReceived;
        private HandleReceivedData<AggTrade> OnAggTradeDataReceived;
        public delegate void HandleReceivedData<TResponse>(TResponse data);



        public WSDataReceiver()
        {
            socket = new();
            subscribeRequestParams = new();
        }

        public async Task StartReceiver()
        {
            if (socket.State == WebSocketState.Open)
                return;

            await socket.ConnectAsync(new Uri("wss://stream.binance.com:9443/ws"), CancellationToken.None);
            await subscribe();
            await receiveData();
        }

        private Task subscribe()
        {
            var param = Utility.BuildWSStreamParam(subscribeRequestParams);
            return socket.SendAsync(param, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        private Task receiveData()
        {
            Task task = new Task(async () =>
            {
                for (;;)
                {
                    var (eventType, data) = await Utility.GetWSStreamReceivedDataAndType(socket);
                    if (eventType == StreamEventTypes.KLINE)
                    {
                        Kline kline = JsonConvert.DeserializeObject<Kline>(data);
                        OnKlineDataReceived(kline);
                    }
                    //Task handleTask = new Task(() => handleDataFunc(jsonString));
                    //handleTask.Start();
                }
            });
            task.Start();
            return task;
        }
        private void addSubscribeParams(string[] sysmbols, string paramType)
        {
            subscribeRequestParams.AddRange(
               sysmbols.Where(s => !subscribeRequestParams.Any(x => x == string.Concat(s, paramType)))
                       .Select(s => string.Concat(s, paramType))
               );
        }
        public WSDataReceiver SubscribeKline(HandleReceivedData<Kline> onKlineDataReceived, params string[] sysmbols)
        {
            OnKlineDataReceived = onKlineDataReceived;
            addSubscribeParams(sysmbols, "@kline_1m");
            return this;
        }
        public WSDataReceiver SubscribeAggTrade(HandleReceivedData<AggTrade> onAggTradeDataReceived, params string[] sysmbols)
        {
            OnAggTradeDataReceived = onAggTradeDataReceived;
            addSubscribeParams(sysmbols, "@aggTrade");
            return this;
        }
    }
}

using Binance.Trading.Bot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Binance.Trading.Bot
{
    public struct Utility
    {
        private static string getWSStreamEventType(string source)
        {
            Regex regex = new Regex("{\"e\":\"(.*?)\",\"");
            var v = regex.Match(source);
            return v.Groups[1].Value;
        }

        public static ArraySegment<byte> BuildWSStreamParam(List<string> subscribeRequestParams)
        {
            var request = new SubscribeRequest { Id = 1, Params = subscribeRequestParams };
            string js = JsonConvert.SerializeObject(request);
            byte[] bytes = Encoding.UTF8.GetBytes(js);
            return new ArraySegment<byte>(bytes);
        }

        public async static Task<(string,string)> GetWSStreamReceivedDataAndEventType(ClientWebSocket socket)
        {
            var recBytes = new byte[2048];
            var arraySegment = new ArraySegment<byte>(recBytes);
            _ = await socket.ReceiveAsync(arraySegment, CancellationToken.None);
            string data = Encoding.UTF8.GetString(recBytes);
            return (getWSStreamEventType(data), data);
        }
    }
}

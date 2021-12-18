using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Binance.Trading.Bot
{
    public class BinanceDataReceiver
    {
        public delegate void HandleReceivedData(string data);

        public async Task StartReceiver(HandleReceivedData handleDataFunc)
        {
            ClientWebSocket socket = new();
            await socket.ConnectAsync(new Uri("wss://stream.binance.com:9443/ws/ethusdt@kline_1m"), CancellationToken.None);
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
    }
}

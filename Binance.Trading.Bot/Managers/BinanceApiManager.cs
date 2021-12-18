using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Trading.Bot.Managers
{
    public class BinanceApiManager
    {
        public void DownloadLast4hrKlines()
        {
            HttpClient httpClient = new();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.binance.com/api/v3/klines?symbol=ETHUSDT&interval=1m&limit=2");
        }
    }
}

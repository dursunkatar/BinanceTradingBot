using System;
namespace Binance.Trading.Bot.Helpers
{
    internal struct DateTimeHelper
    {
        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
            return dateTime;
        }
    }
}

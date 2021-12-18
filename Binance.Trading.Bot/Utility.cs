using System.Text.RegularExpressions;

namespace Binance.Trading.Bot
{
    public struct Utility
    {
        public static string GetStreamEventType(string source)
        {
            Regex regex = new Regex("{\"e\":\"(.*?)\",\"");
            var v = regex.Match(source);
            return v.Groups[1].Value;
        }
    }
}

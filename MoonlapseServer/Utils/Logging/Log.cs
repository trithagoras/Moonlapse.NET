using System;

namespace MoonlapseServer.Utils.Logging
{
    public enum LogContext
    {
        Info, Warn, Error
    }

    public static class Logging
    {
        public static void Log(string client, string message, LogContext context, string userState)
        {
            var now = DateTime.UtcNow;
            Console.WriteLine($"{now.Year:D2}-{now.Month:D2}-{now.Day:D2}T{now.Hour:D2}:{now.Minute:D2}:{now.Second:D2} | {context.ToString().ToUpper()} | clientId={client} | userState={userState} | {message}");
        }
    }
}

using System;

namespace MoonlapseServer.Utils.Logging
{
    public enum LogContext
    {
        Info, Warn, Error
    }

    public static class Logging
    {
        public static void Log(string client, string message, LogContext context)
        {
            var now = DateTime.UtcNow;
            Console.WriteLine($"{now.Year:D2}-{now.Month:D2}-{now.Day:D2}T{now.Hour:D2}:{now.Minute:D2}:{now.Second:D2} | {context.ToString().ToUpper()} | clientId={client} | {message}");
        }
    }
}


//class LogContext :
//    INFO = "INFO"
//    WARN = "WARN"
//    ERROR = "ERROR"


//def log(client: str, s: str, logcontext: LogContext) -> None:
//now = datetime.datetime.utcnow()
//    print(f"{now.year}-{now.month:02}-{now.day:02}T{now.hour:02}:{now.minute:02}:{now.second:02} | {logcontext} | clientId={client} | {s}")

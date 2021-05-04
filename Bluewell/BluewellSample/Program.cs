using SwallowNest.Bluewell;
using System;

namespace SwallowNest.Bluewell.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Printer += LogUtility.OneLinePrinter(Console.WriteLine);
            Log.OutputLogLevel = LogLevel.DEBUG;

            Log.Trace("Trace");
            Log.Debug("Debug");
            Log.Info("Info");
            Log.Warn("Warn");
            Log.Error("Error");
        }
    }
}

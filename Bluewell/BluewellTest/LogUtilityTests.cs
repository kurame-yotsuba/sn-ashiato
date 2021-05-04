using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SwallowNest.Bluewell.Tests
{
    [TestClass]
    public class LogUtilityTests
    {
        /// <summary>
        /// ログの出力先
        /// </summary>
        private List<string> log;

        private readonly string sampleText = "sample text";

        [TestInitialize]
        public void TestInit()
        {
            log = new List<string>();
        }

        [TestMethod]
        public void OneLinePrinterTest()
        {
            Log.Printer += LogUtility.OneLinePrinter(logText => log.Add(logText));
            Log.OutputLogLevel = LogLevel.DEBUG;

            string nowStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string[] logs = new[]{
                $"{nowStr} DEBUG > {sampleText}",
                $"{nowStr}  INFO > {sampleText}",
            };

            Log.Debug(sampleText);
            Log.Info(sampleText);

            log.Is(logs);
        }
    }
}
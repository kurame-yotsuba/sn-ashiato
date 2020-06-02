using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwallowNest.Ashiato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwallowNest.Ashiato.Tests
{
	[TestClass]
	public class LogUtilityTests
	{
		/// <summary>
		/// ログの出力先
		/// </summary>
		List<string> log;
		readonly string sampleText = "sample text";

		void SamplePrinter(string logText, LogLevel logLevel)
		{
			log.Add(logText);
		}

		[TestInitialize]
		public void TestInitialize()
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
			Log.Print(sampleText, LogLevel.INFO);

			log.Is(logs);
		}

		[TestMethod]
		[DataRow(LogLevel.DEBUG)]
		[DataRow(LogLevel.TRACE)]
		[DataRow(LogLevel.INFO)]
		[DataRow(LogLevel.WARN)]
		[DataRow(LogLevel.ERROR)]
		public void OneLineParseTest(LogLevel logLevel)
		{
			Log.Printer += LogUtility.OneLinePrinter(logText => log.Add(logText));
			Log.OutputLogLevel = LogLevel.DEBUG;

			string nowStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
			string logText = "sample";
			Log.Print(logText, logLevel);
			LogInfo	logInfo = LogUtility.OneLineParse(log[0]);

			logInfo.Text.Is(logText);
			logInfo.Level.Is(logLevel);
			logInfo.Time.ToString("yyyy/MM/dd HH:mm:ss").Is(nowStr);
		}
	}
}
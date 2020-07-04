using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
			LogInfo logInfo = LogUtility.OneLineParse(log[0]);

			logInfo.Text.Is(logText);
			logInfo.Level.Is(logLevel);
			logInfo.Time.ToString("yyyy/MM/dd HH:mm:ss").Is(nowStr);
		}

		[TestMethod]
		public void OneLineParseで不正なフォーマットは例外()
		{
			Log.Printer += LogUtility.OneLinePrinter(logText => log.Add(logText));
			Log.OutputLogLevel = LogLevel.DEBUG;

			AssertEx.Throws<FormatException>(() =>
			{
				LogInfo logInfo = LogUtility.OneLineParse("sample");
			});
		}

		[TestMethod]
		public void OneLineTryParseで不正なフォーマットはfalse()
		{
			Log.Printer += LogUtility.OneLinePrinter(logText => log.Add(logText));
			Log.OutputLogLevel = LogLevel.DEBUG;

			string nowStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
			string logText = "sample";
			LogLevel logLevel = LogLevel.INFO;
			Log.Print(logText, logLevel);

			bool success = LogUtility.OneLineTryParse(log[0], out LogInfo logInfo);
			success.IsTrue();
			logInfo.Text.Is(logText);
			logInfo.Level.Is(logLevel);
			logInfo.Time.ToString("yyyy/MM/dd HH:mm:ss").Is(nowStr);

			bool success2 = LogUtility.OneLineTryParse("", out LogInfo logInfo2);
			success2.IsFalse();
			logInfo2.Is(default(LogInfo));
		}

		[TestMethod]
		public void OneLine用のRefleshハンドラ作成()
		{
			Log.Printer += LogUtility.OneLinePrinter(logText => log.Add(logText));
			Log.OutputLogLevel = LogLevel.DEBUG;
			Log.Reflesh += LogUtility.OneLineRotateReflesh(() => log, TimeSpan.FromSeconds(1), log.Clear, x => log.Add(x));

			string logText = "sample";
			LogLevel logLevel = LogLevel.INFO;
			Log.Print(logText, logLevel);
			log.Count.Is(1);

			Task.Delay(2000).Wait();
			Log.Print(logText, logLevel);
			log.Count.Is(1, "さっきのログは消えているはず");
		}
	}
}
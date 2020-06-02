using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwallowNest.Ashiato;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwallowNest.Ashiato.Tests
{
	[TestClass()]
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

		[TestMethod()]
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

			CollectionAssert.AreEqual(logs, log);
		}
	}
}
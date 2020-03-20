using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwallowNest.Ashiato;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwallowNest.Ashiato.Tests
{
	[TestClass()]
	public class PrinterCreatorTests
	{
		/// <summary>
		/// ログの出力先
		/// </summary>
		List<string> log;
		Logger logger;
		readonly string sampleText = "sample text";

		void SamplePrinter(string logText, LogLevel logLevel)
		{
			log.Add(logText);
		}

		[TestInitialize]
		public void TestInitialize()
		{
			log = new List<string>();
			logger = new Logger();
		}

		[TestMethod()]
		public void OneLinePrinterTest()
		{
			logger.Printer += PrinterCreator.OneLinePrinter(logText => log.Add(logText));
			logger.OutputLogLevel = LogLevel.DEBUG;

			string nowStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
			string[] logs = new[]{
				$"{nowStr} DEBUG > {sampleText}",
				$"{nowStr}  INFO > {sampleText}",
			};

			logger.Debug(sampleText);
			logger.Print(sampleText, LogLevel.INFO);

			CollectionAssert.AreEqual(logs, log);
		}
	}
}
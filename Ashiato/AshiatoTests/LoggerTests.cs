using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwallowNest.Ashiato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwallowNest.Ashiato.Tests
{
	[TestClass()]
	public class LoggerTests
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
		public void 単純なログ出力()
		{
			logger.Printer += SamplePrinter;
			for(int i = 0; i < 10; i++)
			{
				logger.Print($"{sampleText} {i}");
			}

			for(int i = 0; i < 10; i++)
			{
				Assert.AreEqual($"{sampleText} {i}", log[i]);
			}
		}

		[TestMethod]
		public void 並行実行()
		{
			int parallelNum = 10;
			int state = 0;

			//並行実行で問題が起きそうな意図的な処理を登録
			logger.Printer += (_, __) =>
			{
				//実行前の状態を取得して
				int before = state;
				//重い処理を行って
				Task.Delay(100).Wait();
				//状態を変更する
				state = before + 1;
			};
			Parallel.For(0, parallelNum, (_, __) => logger.Print(""));

			Assert.AreEqual(parallelNum, state);
		}

		[DataTestMethod]
		[DataRow(LogLevel.INFO)]
		[DataRow(LogLevel.WARN)]
		[DataRow(LogLevel.ERROR)]
		public void OutputLogLevel以上のlevelをPrintできるか(LogLevel level)
		{
			logger.Printer += SamplePrinter;
			logger.OutputLogLevel = LogLevel.INFO;

			logger.Print(sampleText, level);

			Assert.AreEqual(sampleText, log[0]);
		}

		[DataTestMethod]
		[DataRow(LogLevel.TRACE)]
		public void OutputLogLevel未満のlevelはPrintしない(LogLevel level)
		{
			logger.Printer += SamplePrinter;
			logger.OutputLogLevel = LogLevel.INFO;

			logger.Print(sampleText, level);

			Assert.AreEqual(0, log.Count);
		}

		[TestMethod]
		public void RefleshTest()
		{
			void 要素が5つ以上だったら先頭のログを削除()
			{
				if (log.Count >= 5)
				{
					log.RemoveAt(0);
				}
			}

			logger.Printer += SamplePrinter;
			logger.Reflesh += 要素が5つ以上だったら先頭のログを削除;

			logger.Print("Info", LogLevel.INFO);
			Assert.AreEqual(1, log.Count);

			logger.Print("Info", LogLevel.INFO);
			Assert.AreEqual(2, log.Count);

			logger.Print("Info", LogLevel.INFO);
			Assert.AreEqual(3, log.Count);

			logger.Print("Info", LogLevel.INFO);
			Assert.AreEqual(4, log.Count);

			logger.Print("Info", LogLevel.INFO);
			Assert.AreEqual(5, log.Count);

			logger.Print("Info", LogLevel.INFO);
			Assert.AreEqual(5, log.Count);

			logger.Print("Info", LogLevel.INFO);
			Assert.AreEqual(5, log.Count);

		}

		[TestMethod]
		public void DebugではLogLevelはDebug固定()
		{
			List<LogLevel> debugLog = new List<LogLevel>();
			logger.Printer += (_, logLevel) => debugLog.Add(logLevel);
			logger.OutputLogLevel = LogLevel.DEBUG;

			logger.Debug(sampleText);

			Assert.AreEqual(LogLevel.DEBUG, debugLog[0]);
		}
	}
}
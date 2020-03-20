using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwallowNest.Ashiato;
using System;
using System.Collections.Generic;
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
		List<string> logBuffer;
		Logger logger;
		string sampleText = "sample text";

		void SamplePrinter(string logText, LogLevel logLevel)
		{
			logBuffer.Add(logText);
		}

		[TestInitialize]
		public void TestInitialize()
		{
			logBuffer = new List<string>();
			logger = new Logger()
			{
				OutputLogLevel = LogLevel.TRACE
			};
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
				Assert.AreEqual($"{sampleText} {i}", logBuffer[i]);
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
	}
}
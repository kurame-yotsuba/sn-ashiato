using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwallowNest.Bluewell.Tests
{
	[TestClass()]
	public class LogTests
	{
		/// <summary>
		/// ログの出力先
		/// </summary>
		private List<string> log;

		private readonly string sampleText = "sample text";

		void SamplePrinter(LogInfo log)
		{
			this.log.Add(log.Text);
		}

		[TestInitialize]
		public void TestInitialize()
		{
			log = new List<string>();
		}

		[TestMethod()]
		public void 単純なログ出力()
		{
			Log.Printer += SamplePrinter;
			for (int i = 0; i < 10; i++)
			{
				Log.Info($"{sampleText} {i}");
			}

			for (int i = 0; i < 10; i++)
			{
				log[i].Is($"{sampleText} {i}");
			}
		}

		[TestMethod]
		public void 並行実行()
		{
			int parallelNum = 10;
			int state = 0;

			//並行実行で問題が起きそうな意図的な処理を登録
			Log.Printer += _ =>
			{
				//実行前の状態を取得して
				int before = state;
				//重い処理を行って
				Task.Delay(100).Wait();
				//状態を変更する
				state = before + 1;
			};
			Parallel.For(0, parallelNum, (_, __) => Log.Info(""));

			state.Is(parallelNum);
		}

		[DataTestMethod]
		public void OutputLogLevel以上のLevelをPrint()
		{
			Log.Printer += SamplePrinter;
			Log.OutputLogLevel = LogLevel.INFO;

			Log.Info(sampleText);
			log[0].Is(sampleText);

			Log.Warn(sampleText);
			log[1].Is(sampleText);

			Log.Error(sampleText);
			log[2].Is(sampleText);
		}

		[DataTestMethod]
		public void OutputLogLevel未満のLevelはPrintしない()
		{
			Log.Printer += SamplePrinter;
			Log.OutputLogLevel = LogLevel.INFO;

			Log.Trace(sampleText);
			Log.Debug(sampleText);

			log.Count.Is(0);
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

			Log.Printer += SamplePrinter;
			Log.Reflesh += 要素が5つ以上だったら先頭のログを削除;

			Log.Info("");
			log.Count.Is(1);

			Log.Info("");
			log.Count.Is(2);

			Log.Info("");
			log.Count.Is(3);

			Log.Info("");
			log.Count.Is(4);

			// これ以上はログが増えない
			Log.Info("");
			log.Count.Is(5);

			Log.Info("");
			log.Count.Is(5);

			Log.Info("");
			log.Count.Is(5);
		}

		[TestMethod]
		public void DebugのLogLevelはDebug()
		{
			List<LogLevel> debugLog = new List<LogLevel>();
			Log.Printer += log => debugLog.Add(log.Level);
			Log.OutputLogLevel = LogLevel.TRACE;

			Log.Trace("");
			Log.Debug("");
			Log.Info("");
			Log.Warn("");
			Log.Error("");

			debugLog.Is(
				LogLevel.TRACE,
				LogLevel.DEBUG,
				LogLevel.INFO,
				LogLevel.WARN,
				LogLevel.ERROR);
		}
	}
}

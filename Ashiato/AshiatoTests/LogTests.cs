using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwallowNest.Ashiato.Tests
{
	[TestClass()]
	public class LogTests
	{
		/// <summary>
		/// ログの出力先
		/// </summary>
		List<string> log;
		readonly string sampleText = "sample text";

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
				Log.Print($"{sampleText} {i}");
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
			Parallel.For(0, parallelNum, (_, __) => Log.Print(""));

			state.Is(parallelNum);
		}

		[DataTestMethod]
		[DataRow(LogLevel.INFO)]
		[DataRow(LogLevel.WARN)]
		[DataRow(LogLevel.ERROR)]
		public void OutputLogLevel以上のlevelをPrintできるか(LogLevel level)
		{
			Log.Printer += SamplePrinter;
			Log.OutputLogLevel = LogLevel.INFO;

			Log.Print(sampleText, level);

			log[0].Is(sampleText);
		}

		[DataTestMethod]
		[DataRow(LogLevel.TRACE)]
		public void OutputLogLevel未満のlevelはPrintしない(LogLevel level)
		{
			Log.Printer += SamplePrinter;
			Log.OutputLogLevel = LogLevel.INFO;

			Log.Print(sampleText, level);

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

			Log.Print("Info", LogLevel.INFO);
			log.Count.Is(1);

			Log.Print("Info", LogLevel.INFO);
			log.Count.Is(2);

			Log.Print("Info", LogLevel.INFO);
			log.Count.Is(3);

			Log.Print("Info", LogLevel.INFO);
			log.Count.Is(4);

			Log.Print("Info", LogLevel.INFO);
			log.Count.Is(5);

			Log.Print("Info", LogLevel.INFO);
			log.Count.Is(5);

			Log.Print("Info", LogLevel.INFO);
			log.Count.Is(5);
		}

		[TestMethod]
		public void DebugではLogLevelはDebug固定()
		{
			List<LogLevel> debugLog = new List<LogLevel>();
			Log.Printer += log => debugLog.Add(log.Level);
			Log.OutputLogLevel = LogLevel.DEBUG;

			Log.Debug(sampleText);

			debugLog[0].Is(LogLevel.DEBUG);
		}
	}
}
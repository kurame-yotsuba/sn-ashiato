using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwallowNest.Ashiato
{
	public static class LogUtility
	{
		private static readonly string timeFormat = "yyyy/MM/dd HH:mm:ss";
		private static readonly string prompt = " > ";

		/// <summary>
		/// ログレベルを文字列にしたときの最大幅です。
		/// </summary>
		public static int MaxLengthOfLogLevelText { get; } =
			Enum.GetNames(typeof(LogLevel)).Select(x => x.Length).Max();

		// 各種range
		private static readonly Range timeRange = 0..timeFormat.Length;
		private static readonly Range levelRange =
			(timeFormat.Length + 1)..(timeFormat.Length + 1 + MaxLengthOfLogLevelText);
		private static readonly Range textRange = (timeFormat.Length + 1 + MaxLengthOfLogLevelText + prompt.Length)..;

		/// <summary>
		/// 1行用のLogPrintHandlerを作成します。
		/// </summary>
		/// <param name="printer"></param>
		/// <returns></returns>
		public static LogPrintHandler OneLinePrinter(Action<string> printer)
		{
			//2018/04/01 12:00:00  INFO > logText
			//2018/04/01 12:00:00 DEBUG > logText
			//みたいな感じになる

			return log =>
			{
				string nowStr = DateTime.Now.ToString(timeFormat);
				string lvlStr = log.Level.ToString().PadLeft(MaxLengthOfLogLevelText);

				string result = $"{nowStr} {lvlStr}{prompt}{log.Text}";

				printer(result);
			};
		}

		/// <summary>
		/// OneLinePrinterで出力されたログをLogInfoに変換します。
		/// </summary>
		/// <param name="lineText"></param>
		/// <returns></returns>
		public static LogInfo OneLineParse(string lineText)
		{
			if (OneLineTryParse(lineText, out LogInfo log))
			{
				return log;
			}
			else
			{
				throw new FormatException("不正なフォーマットのOneLineLogです。");
			}
		}

		/// <summary>
		/// OneLinePrinterで出力されたログをLogInfoに変換します。
		/// </summary>
		/// <param name="lineText"></param>
		/// <param name="log"></param>
		/// <returns></returns>
		public static bool OneLineTryParse(string lineText, out LogInfo log)
		{
			log = default;
			bool fail;
			fail = lineText.Length < timeFormat.Length + 1 + MaxLengthOfLogLevelText + prompt.Length;
			if (fail) { return false; }

			ReadOnlySpan<char> span = lineText.AsSpan();

			fail = !DateTime.TryParse(span[timeRange], out DateTime time);
			if (fail) { return false; }

			var hoge = span[levelRange].ToString();
			fail = !Enum.TryParse(span[levelRange].ToString(), out LogLevel level);
			if (fail) { return false; }

			string text = span[textRange].ToString();

			log = new LogInfo(text, level, time);
			return true;
		}
	}
}

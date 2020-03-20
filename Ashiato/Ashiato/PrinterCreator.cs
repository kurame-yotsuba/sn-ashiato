using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwallowNest.Ashiato
{
	public static class PrinterCreator
	{
		private static readonly string timeFormat = "yyyy/MM/dd HH:mm:ss";

		/// <summary>
		/// ログレベルを文字列にしたときの最大幅です。
		/// </summary>
		public static int MaxLengthOfLogLevelText { get; } =
			Enum.GetNames(typeof(LogLevel)).Select(x => x.Length).Max();

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

			return (logText, logLevel) =>
			{
				var nowStr = DateTime.Now.ToString(timeFormat);
				var lvlStr = $" {logLevel.ToString().PadLeft(MaxLengthOfLogLevelText)} > ";

				var result = nowStr + lvlStr + logText;

				printer(result);
			};
		}
	}
}

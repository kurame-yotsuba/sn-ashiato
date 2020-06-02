﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwallowNest.Ashiato
{
	public static class LogUtility
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

			return log =>
			{
				string nowStr = DateTime.Now.ToString(timeFormat);
				string lvlStr = log.Level.ToString().PadLeft(MaxLengthOfLogLevelText);

				string result = $"{nowStr} {lvlStr} > {log.Text}";

				printer(result);
			};
		}
	}
}

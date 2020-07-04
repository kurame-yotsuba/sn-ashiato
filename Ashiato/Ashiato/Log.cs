using System;
using System.Diagnostics;

namespace SwallowNest.Ashiato
{
	public static class Log
	{
		#region private member

		/// <summary>
		/// 排他制御用のオブジェクト
		/// </summary>
		private static readonly object syncObject = new object();

		/// <summary>
		/// ログを出力します。
		/// </summary>
		/// <param name="logText"></param>
		/// <param name="logLevel"></param>
		static void Print(string logText, LogLevel logLevel)
		{
			if (Printer is null) { return; }

			//logLevelの重要度がLogLevel以上の場合にログを出力する
			if (logLevel >= OutputLogLevel)
			{
				lock (syncObject)
				{
					Reflesh?.Invoke();
					LogInfo log = new LogInfo(logText, logLevel, DateTime.Now);
					Printer(log);
				}
			}
		}

		#endregion private member

		#region public member

		/// <summary>
		/// 出力するログのレベル
		/// </summary>
		public static LogLevel OutputLogLevel { set; get; } = LogLevel.INFO;

		/// <summary>
		/// ログ出力のイベントハンドラー
		/// </summary>
		public static event LogPrintHandler? Printer;

		/// <summary>
		/// ログ履歴をリフレッシュするイベントハンドラー
		/// </summary>
		public static event LogRefreshHandler? Reflesh;

		/// <summary>
		/// TRACE定数が定義されているときのみ、ログを出力します。
		/// </summary>
		/// <param name="logText"></param>
		[Conditional(nameof(LogLevel.TRACE))]
		public static void Trace(string logText) => Print(logText, LogLevel.TRACE);

		/// <summary>
		/// DEBUG定数が定義されているときのみ、ログを出力します。
		/// </summary>
		/// <param name="logText"></param>
		[Conditional(nameof(LogLevel.DEBUG))]
		public static void Debug(string logText) => Print(logText, LogLevel.DEBUG);

		/// <summary>
		/// 情報ログを出力します。
		/// </summary>
		/// <param name="logText"></param>
		public static void Info(string logText) => Print(logText, LogLevel.INFO);

		/// <summary>
		/// 警告ログを出力します。
		/// </summary>
		/// <param name="logText"></param>
		public static void Warn(string logText) => Print(logText, LogLevel.WARN);

		/// <summary>
		/// エラーログを出力します。
		/// </summary>
		/// <param name="logText"></param>
		public static void Error(string logText) => Print(logText, LogLevel.ERROR);

		#endregion public member
	}
}

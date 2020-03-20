﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwallowNest.Ashiato
{
	public class Logger
	{
		#region private member

		/// <summary>
		/// 排他制御用のオブジェクト
		/// </summary>
		private readonly object syncObject = new object();

		#endregion

		/// <summary>
		/// 出力するログのレベル
		/// </summary>
		public LogLevel OutputLogLevel { set; get; } = LogLevel.TRACE;

		/// <summary>
		/// ログ出力のイベントハンドラー
		/// </summary>
		public event LogPrintHandler? Printer;

		/// <summary>
		/// ログ履歴をリフレッシュするイベントハンドラー
		/// </summary>
		public event LogRefleshHandler? Reflesh;

		/// <summary>
		/// ログを出力します。
		/// </summary>
		/// <param name="logText"></param>
		/// <param name="logLevel"></param>
		public void Print(string logText, LogLevel logLevel = LogLevel.INFO)
		{
			if (Printer is null) { return; }

			//logLevelの重要度がLogLevel以上の場合にログを出力する
			if (logLevel >= OutputLogLevel)
			{
				lock (syncObject)
				{
					Reflesh?.Invoke();
					Printer(logText, logLevel);
				}
			}
		}

		/// <summary>
		/// デバッグビルドでのみ、ログを出力します。
		/// </summary>
		/// <param name="logText"></param>
		[Conditional("DEBUG")]
		public void Debug(string logText) => Print(logText, LogLevel.DEBUG);
	}
}

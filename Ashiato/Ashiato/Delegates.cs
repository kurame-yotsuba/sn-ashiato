using System;
using System.Collections.Generic;
using System.Text;

namespace SwallowNest.Ashiato
{
	/// <summary>
	/// 文字列とログレベルを受け取るイベントハンドラーを表す型です。
	/// </summary>
	/// <param name="logText"></param>
	/// <param name="logLevel"></param>
	public delegate void LogPrintHandler(string logText, LogLevel logLevel);

	/// <summary>
	/// ログのリフレッシュを行うイベントハンドラーを表す型です。
	/// </summary>
	public delegate void LogRefleshHandler();

	/// <summary>
	/// 文字列とログレベルを受け取って、
	/// ログ用にフォーマットされた文字列を返す型です。
	/// </summary>
	/// <param name="logText"></param>
	/// <param name="logLevel"></param>
	/// <returns></returns>
	public delegate string LogFormatter(string logText, LogLevel logLevel);
}

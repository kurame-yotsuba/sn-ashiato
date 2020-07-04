namespace SwallowNest.Ashiato
{
	/// <summary>
	/// 文字列とログレベルを受け取るイベントハンドラーを表す型です。
	/// </summary>
	/// <param name="log"></param>
	public delegate void LogPrintHandler(LogInfo log);

	/// <summary>
	/// ログのリフレッシュを行うイベントハンドラーを表す型です。
	/// </summary>
	public delegate void LogRefleshHandler();

	/// <summary>
	/// 文字列とログレベルを受け取って、
	/// ログ用にフォーマットされた文字列を返す型です。
	/// </summary>
	/// <param name="log"></param>
	/// <returns></returns>
	public delegate string LogFormatter(LogInfo log);
}

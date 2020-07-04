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
	public delegate void LogRefreshHandler();
}

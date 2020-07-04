namespace SwallowNest.Ashiato
{
	/// <summary>
	/// 表示するログの詳しさを表します。
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// 重要度の低いログ
		/// </summary>
		TRACE,

		/// <summary>
		/// デバッグ用のログ
		/// </summary>
		DEBUG,

		/// <summary>
		/// 重要度の高いログ
		/// </summary>
		INFO,

		/// <summary>
		/// 警告を表すログ
		/// </summary>
		WARN,

		/// <summary>
		/// 例外時のログ
		/// </summary>
		ERROR,
	}
}

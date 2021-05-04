using System;

namespace SwallowNest.Bluewell
{
    /// <summary>
    /// ログの情報をまとめた構造体
    /// </summary>
    public readonly struct LogInfo
    {
        /// <summary>
        /// ログテキスト
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// ログレベル
        /// </summary>
        public LogLevel Level { get; }

        /// <summary>
        /// ログを出力した時刻
        /// </summary>
        public DateTime Time { get; }

        internal LogInfo(string text, LogLevel level, DateTime time)
        {
            Text = text;
            Level = level;
            Time = time;
        }
    }
}
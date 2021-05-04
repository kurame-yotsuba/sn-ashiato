using System;
using System.Diagnostics;

namespace SwallowNest.Bluewell
{
    /// <summary>
    /// 簡易的なログ出力を行うクラス。
    /// </summary>
    public static class Log
    {
        #region private member

        /// <summary>
        /// 排他制御用のオブジェクト
        /// </summary>
        private static readonly object sync = new();

        /// <summary>
        /// ログ出力の共通処理
        /// </summary>
        /// <param name="logText"></param>
        /// <param name="logLevel"></param>
        private static void Print(string logText, LogLevel logLevel)
        {
            // プリント系のメソッドの実行するインナーメソッド
            static void Invoke(LogInfo log)
            {
                Reflesh?.Invoke();
                Printer?.Invoke(log);
            }

            if (Printer is null) { return; }

            //logLevelの重要度がLogLevel以上の場合にログを出力する
            if (logLevel >= OutputLogLevel)
            {
                LogInfo log = new(logText, logLevel, DateTime.Now);

                // 排他制御するかしないか
                if (IsThreadSafe)
                {
                    lock (sync)
                    {
                        Invoke(log);
                    }
                }
                else
                {
                    Invoke(log);
                }
            }
        }

        #endregion private member

        #region public member

        /// <summary>
        /// ログ出力時に排他制御を行うか否かのフラグ。
        ///
        /// <list type="bullet">
        ///		<item>初期値：false</item>
        /// </list>
        /// </summary>
        public static bool IsThreadSafe { get; set; } = false;

        /// <summary>
        /// 出力するログのレベル。
        ///
        /// <list type="bullet">
        ///		<item>初期値：<see cref="LogLevel.INFO"/></item>
        /// </list>
        /// </summary>
        public static LogLevel OutputLogLevel { get; set; } = LogLevel.INFO;

        /// <summary>
        /// ログ出力のイベントハンドラー
        /// </summary>
        public static event Action<LogInfo>? Printer;

        /// <summary>
        /// ログ履歴をリフレッシュするイベントハンドラー
        /// </summary>
        public static event Action? Reflesh;

        /// <summary>
        /// TRACE定数が定義されているときのみ、ログを出力します。
        /// </summary>
        /// <param name="logText"></param>
        [Conditional("TRACE")]
        public static void Trace(string logText) => Print(logText, LogLevel.TRACE);

        /// <summary>
        /// DEBUG定数が定義されているときのみ、ログを出力します。
        /// </summary>
        /// <param name="logText"></param>
        [Conditional("DEBUG")]
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
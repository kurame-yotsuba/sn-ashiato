using System;
using System.Collections.Generic;
using System.Text;

namespace SwallowNest.Ashiato
{
	public struct LogInfo
	{
		public string Text { get; }
		public LogLevel Level { get; }
		public DateTime Time { get; }

		internal LogInfo(string text, LogLevel level, DateTime time)
		{
			Text = text;
			Level = level;
			Time = time;
		}
	}
}

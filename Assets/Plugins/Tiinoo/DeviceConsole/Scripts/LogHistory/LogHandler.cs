using System;
using UnityEngine;
using System.Collections;
using System.Diagnostics;

namespace Tiinoo.DeviceConsole
{
	public static class LogHandler
	{
        public static System.Action<LogEntry> onLogAdded;
		public static System.Action onExceptionOccur;
		
		private static LogBuffer m_logBuffer = new LogBuffer();

        public static LogBuffer LogBuffer
		{
			get {return m_logBuffer;}
		}

		public static void LogCallback(string logString, string stackTrace, LogType type)
		{
			LogEntry log = new LogEntry(type, logString, stackTrace);
			m_logBuffer.Add(log);

			if (onLogAdded != null)
			{
				onLogAdded(log);
			}

			if (log.logType == LogType.Exception)
			{
				if (onExceptionOccur != null)
				{
					onExceptionOccur();
				}
			}
		}
	}
}


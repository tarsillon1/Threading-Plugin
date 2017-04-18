using UnityEngine;
using System.Collections;
using System;

namespace Tiinoo.DeviceConsole
{
	public class LogEntry
	{
		private const int SHORT_MESSAGE_MAX_LENGTH = 128;

		private LogType m_logType;
		private string m_message;
		private string m_stackTrace;

		private bool m_isWatched;
		private string m_lowercaseMessage;
		private string m_shortMessage;

//		private string m_timeStamp;

		public LogEntry(LogType logType, string logString, string logStackTrace)
		{
			m_logType = logType;

			bool hasWatchFlag = false;
			m_message = (logString != null) ? logString : "";
			m_message = DCWatcher.RemoveWatchFlagIfHas(m_message, ref hasWatchFlag);
			m_lowercaseMessage = m_message.ToLower();
			m_shortMessage = CreateShortMessage(m_message);

			m_isWatched = hasWatchFlag;
			m_stackTrace = (logStackTrace != null) ? logStackTrace : "";

//			m_timeStamp = string.Format("{0:HH:mm:ss.ffff}", DateTime.Now);
		}

		public LogType logType
		{
			get {return m_logType;}
		}

		public string message
		{
			get {return m_message;}
		}

		public string stackTrace
		{
			get {return m_stackTrace;}
		}

		public bool isWatched
		{
			get {return m_isWatched;}
		}

		public string lowercaseMessage
		{
			get {return m_lowercaseMessage;}
		}

		public string shortMessage
		{
			get {return m_shortMessage;}
		}

		private static string CreateShortMessage(string message)
		{
			string shortMessage = "";
			if (!string.IsNullOrEmpty(message))
			{
				string[] strs = message.Split('\n');
				shortMessage = strs[0];

				if (shortMessage.Length > SHORT_MESSAGE_MAX_LENGTH)
				{
					shortMessage = shortMessage.Substring(0, SHORT_MESSAGE_MAX_LENGTH);
				}
			}
			return shortMessage;
		}

		public override string ToString()
		{
			return m_message;
		}
	}
}


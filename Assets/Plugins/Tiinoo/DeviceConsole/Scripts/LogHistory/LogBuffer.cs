using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Tiinoo.DeviceConsole
{
	public class LogBuffer
	{
		private LinkedList<LogEntry> m_logs;
		
		public LogBuffer()
		{
			m_logs = new LinkedList<LogEntry>();
		}
		
		public void Add(LogEntry log)
		{
			m_logs.AddLast(log);
		}

		public LogEntry RemoveFirst()
		{
			LogEntry removedLog = null;

			LinkedListNode<LogEntry> node = m_logs.First;
			if (node != null)
			{
				removedLog = node.Value;
				m_logs.RemoveFirst();
			}

			return removedLog;
		}
		
		public void Clear()
		{
			m_logs.Clear();
		}
		
		public int Count
		{
			get {return m_logs.Count;}
		}
		
		public LinkedList<LogEntry> Logs
		{
			get {return m_logs;}
		}
	}
}

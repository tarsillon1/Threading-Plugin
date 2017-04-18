using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using System;
using Tiinoo.DeviceConsole;

namespace Tiinoo.DeviceConsole
{
	public class WindowConsole : MonoBehaviour
	{
		#region inspector
		public GameObject objConsole;

		public GameObject btnToMaxSize;
		public GameObject btnToNormalSize;

		public Text uiInfoCount;
		public Text uiWarningCount;
		public Text uiErrorCount;

		public InputField uiFilter;

		public ItemVerticalLayoutGroup consoleLayoutGroup;

		public ScrollRect stackTraceScrollRect;
		public Text uiStackTrace;

		public Text uiStatus;

		public Sprite iconInfo;
		public Sprite iconWarning;
		public Sprite iconError;
		public Color bgLogLight;
		public Color bgLogDark;
		public Color bgLogSelected;
		#endregion

		public static bool isVisible;

		private bool m_isInited;

		private bool m_isShowInfo = true;
		private bool m_isShowWarning = true;
		private bool m_isShowError = true;
		private bool m_isShowWatchedLogsOnly = false;
		
		private int m_infoCount;
		private int m_warningCount;
		private int m_errorCount;
		
		private RectTransform m_consoleRectTransform;
		private float m_consoleNormalHeight;

		private const string METHOD_DO_FILTER = "FilterLogsAndRefresh";
		private string m_searchText = "";
		private bool m_isSearchTextAtBegin = false;

		private LogBuffer m_filteredLogBuffer = new LogBuffer();

		private bool m_isDirty;

		private const int STACK_TRACE_MAX_LENGTH = 1200;
		private const string STR_NO_STACK_TRACE = "<-- Stack trace is not available. Please try a development build. -->";
		private const string STR_ETC = "< ... etc ... >";

		#region singleton
		private static WindowConsole m_instance;
		
		public static WindowConsole Instance
		{
			get {return m_instance;}
		}
		
		void Awake()
		{
			m_instance = this;
		}
		#endregion
		
		void OnEnable()
		{
			isVisible = true;
			LogHandler.onLogAdded += HandleOnLogAdded;
			consoleLayoutGroup.onItemSelected += HandleOnLogSelected;

			if (!m_isInited)
			{
				Init();
				m_isInited = true;
			}

			if (m_isInited)
			{
				FilterLogsAndRefresh();
			}
		}
		
		void OnDisable()
		{
			isVisible = false;
			LogHandler.onLogAdded -= HandleOnLogAdded;
			consoleLayoutGroup.onItemSelected -= HandleOnLogSelected;
		}

		void Update()
		{
			if (m_isDirty)
			{
				Refresh();
				m_isDirty = false;
			}
		}

		void Init()
		{
			m_consoleRectTransform = objConsole.GetComponent<RectTransform>();
			m_consoleNormalHeight = m_consoleRectTransform.sizeDelta.y;
			
			StretchToNormalSize();
		}

		private void MarkDirty()
		{
			m_isDirty = true;
		}

		#region action
		private void HandleOnLogAdded(LogEntry log)
		{
			bool isPassed = StatisticAndFilter(log);
			if (isPassed)
			{
				consoleLayoutGroup.AddItem(log);
			}
			RefreshLogCount();
		}
		
		private void HandleOnLogSelected(object item)
		{
			LogEntry log = item as LogEntry;
			SetStackTraceMsg(log);
		}

		private void SetStackTraceMsg(LogEntry log)
		{
			uiStackTrace.text = CreateStackTraceMsg(log);
			UGUIUtil.ScrollToLeftTop(stackTraceScrollRect);
		}

		private static string CreateStackTraceMsg(LogEntry log)
		{
			string msg = "";
			if (log != null)
			{
				if (string.IsNullOrEmpty(log.stackTrace))
				{
					msg = log.message + "\n" + STR_NO_STACK_TRACE;
				}
				else
				{
					msg = log.message + "\n" + log.stackTrace;
				}
				
				if (msg.Length > STACK_TRACE_MAX_LENGTH)
				{
					msg = msg.Substring(0, STACK_TRACE_MAX_LENGTH);
					msg = msg + "\n" + STR_ETC;
				}
			}
			return msg;
		}

		public void ShowHideInfo(bool isShow)
		{
			m_isShowInfo = isShow;
			FilterLogsAndRefresh();
		}
		
		public void ShowHideWarning(bool isShow)
		{
			m_isShowWarning = isShow;
			FilterLogsAndRefresh();
		}
		
		public void ShowHideError(bool isShow)
		{
			m_isShowError = isShow;
			FilterLogsAndRefresh();
		}

		public void ToggleWatchedLogsOnly(bool isShowWatchedLogsOnly)
		{
			m_isShowWatchedLogsOnly = isShowWatchedLogsOnly;
			FilterLogsAndRefresh();
		}
		
		public void Clean()
		{
			LogHandler.LogBuffer.Clear();
			m_filteredLogBuffer.Clear();
			SetStackTraceMsg(null);
			FilterLogsAndRefresh();
		}
		
		public void StretchToNormalSize()
		{
			btnToNormalSize.SetActive(false);
			btnToMaxSize.SetActive(true);
			m_consoleRectTransform.sizeDelta = new Vector2(m_consoleRectTransform.sizeDelta.x, m_consoleNormalHeight);
		}
		
		public void StretchToMaxSize()
		{
			btnToNormalSize.SetActive(true);
			btnToMaxSize.SetActive(false);
			m_consoleRectTransform.sizeDelta = new Vector2(m_consoleRectTransform.sizeDelta.x, UIWindowMgr.Instance.CanvasHeight);
		}
		
		public void HandleOnSearchStringChange(string s)
		{
			m_searchText = s.ToLower();
			if (IsInvoking(METHOD_DO_FILTER))
			{
				CancelInvoke(METHOD_DO_FILTER);
			}
			Invoke(METHOD_DO_FILTER, 0.3f);
		}

		public void ClearFilterString()
		{
			uiFilter.text = "";	// This will trigger a InputFiled.onValueChanged event, which will call HandleOnFilterStringChange()
		}

		private bool HasSearchText()
		{
			if (string.IsNullOrEmpty(m_searchText))
			{
				return false;
			}
			return true;
		}

		public void HandleOnSearchTextAtBeginChange(bool val)
		{
			m_isSearchTextAtBegin = val;

			if (!HasSearchText())
			{
				return;
			}

			if (IsInvoking(METHOD_DO_FILTER))
			{
				return;
			}

			FilterLogsAndRefresh();
		}

		public void Close()
		{
			SetStackTraceMsg(null);
			UIWindowMgr.Instance.CloseCurrentWindow();
		}
		#endregion

		private void Refresh()
		{
			object curSelectedItem = consoleLayoutGroup.GetCurSelectedItem();
			consoleLayoutGroup.ClearItems();

			LinkedList<LogEntry> logs = m_filteredLogBuffer.Logs;
			foreach (LogEntry log in logs)
			{
				consoleLayoutGroup.AddItem(log);
			}

			if (curSelectedItem != null)
			{
				consoleLayoutGroup.SetCurSelectedItem(curSelectedItem);
			}

			RefreshLogCount();
		}
		
		private void RefreshLogCount()
		{
			uiInfoCount.text = CreateLogTypeText(m_infoCount);
			uiWarningCount.text = CreateLogTypeText(m_warningCount);
			uiErrorCount.text = CreateLogTypeText(m_errorCount);
		}
		
		private static string CreateLogTypeText(int logCount)
		{
			string s = (logCount > 999) ? "999+" : (logCount + "");
			return s;
		}
		
		private void ResetLogCount()
		{
			m_infoCount = 0;
			m_warningCount = 0;
			m_errorCount = 0;
		}
		
		private void StatisticLogCount(LogEntry log)
		{
			LogType logType = log.logType;
			if (logType == LogType.Log)
			{
				m_infoCount++;
			}
			else if (logType == LogType.Warning)
			{
				m_warningCount++;
			}
			else if (logType == LogType.Error || logType == LogType.Exception || logType == LogType.Assert)
			{
				m_errorCount++;
			}
		}

		private bool FilterPassCheck1(LogEntry log)
		{
			if (m_isShowWatchedLogsOnly && !log.isWatched)
			{
				return false;
			}

			if (HasSearchText())
			{
				if (!m_isSearchTextAtBegin && !log.lowercaseMessage.Contains(m_searchText))
				{
					return false;
				}
				else if (m_isSearchTextAtBegin && !log.lowercaseMessage.StartsWith(m_searchText))
				{
					return false;
				}
			}

			return true;
		}
		
		private bool FilterPassCheck2(LogEntry log)
		{
			LogType logType = log.logType;
			if ((logType == LogType.Log && !m_isShowInfo)
			    || (logType == LogType.Warning && !m_isShowWarning)
			    || (logType == LogType.Error && !m_isShowError)
			    || (logType == LogType.Exception && !m_isShowError)
			    || (logType == LogType.Assert && !m_isShowError))
			{
				return false;
			}

			return true;
		}

		private void FilterLogsAndRefresh()
		{
			FilterLogs();
			MarkDirty();
		}

		private void FilterLogs()
		{
			m_filteredLogBuffer.Clear();
			ResetLogCount();

			LinkedList<LogEntry> logs = LogHandler.LogBuffer.Logs;
			foreach (LogEntry log in logs)
			{
				StatisticAndFilter(log);
			}
		}

		private bool StatisticAndFilter(LogEntry log)
		{
			bool isPassed = FilterPassCheck1(log);
			if (isPassed)
			{
				StatisticLogCount(log);
				isPassed = FilterPassCheck2(log);
				if (isPassed)
				{
					m_filteredLogBuffer.Add(log);
				}
			}
			return isPassed;
		}
	}
}



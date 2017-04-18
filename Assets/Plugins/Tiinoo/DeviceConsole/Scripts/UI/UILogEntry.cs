using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Tiinoo.DeviceConsole;

namespace Tiinoo.DeviceConsole
{
	public class UILogEntry : MonoBehaviour, ItemDrawer
	{
		private static Color White_Transparent = new Color(1, 1, 1, 0);

		#region inspector
		public Image uiBg;
		public Image uiLogType;
		public Text uiLogMessage;
		public Image uiIsWatched;
		#endregion
		
		private LogEntry m_log;

		public void Draw(object item, bool isOddRow, bool isSelected)
		{
			LogEntry log = item as LogEntry;
			if (log == null)
			{
				return;
			}

			m_log = log;
			uiLogType.sprite = GetLogTypeSprite(m_log.logType);
			uiLogMessage.text = m_log.shortMessage;
			uiIsWatched.color = m_log.isWatched ? Color.white : White_Transparent;
			uiBg.color = GetBgColor(isOddRow, isSelected);
		}
		
		public static Color GetBgColor(bool isOddRow, bool isSelected)
		{
			WindowConsole console = WindowConsole.Instance;
			Color color;
			if (isSelected)
			{
				color = console.bgLogSelected;
			}
			else
			{
				color = isOddRow ? console.bgLogDark : console.bgLogLight;
			}
			return color;
		}
		
		public static Sprite GetLogTypeSprite(LogType logType)
		{
			Sprite sprite = null;
			
			switch (logType)
			{
			case LogType.Log:
				sprite = WindowConsole.Instance.iconInfo;
				break;
				
			case LogType.Warning:
				sprite = WindowConsole.Instance.iconWarning;
				break;
				
			case LogType.Error:
			case LogType.Exception:
			case LogType.Assert:
				sprite = WindowConsole.Instance.iconError;
				break;
				
			default:
				sprite = WindowConsole.Instance.iconInfo;
				break;
			}
			
			return sprite;
		}
	}
}



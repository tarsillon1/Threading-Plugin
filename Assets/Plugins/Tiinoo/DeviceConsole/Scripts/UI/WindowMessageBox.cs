using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Tiinoo.DeviceConsole
{
	public class WindowMessageBox : MonoBehaviour 
	{
		public Text uiTitle;
		public Text uiMessage;
		public Text uiOKText;
		public Text uiCancelText;

		private System.Action m_okCallback;
		private System.Action m_cancelCallback;

		// if title, message, ok, cancel is null, it means use default value.
		// if okcallback, cancelCallback is null, it means no callback.
		public void ShowMessageBox(string title, string message, string ok, string cancel, System.Action okCallback, System.Action cancelCallback)
		{
			uiTitle.text = StringUtil.GetNotNullString(title, "Title");
			uiMessage.text = StringUtil.GetNotNullString(message, "Message");
			uiOKText.text = StringUtil.GetNotNullString(ok, "OK");
			uiCancelText.text = StringUtil.GetNotNullString(cancel, "Cancel");

			m_okCallback = okCallback;
			m_cancelCallback = cancelCallback;

			UIWindowMgr.Instance.PopUpWindow(UIWindow.Id.MessageBox);
		}

		public void ClickOK()
		{
			UIWindowMgr.Instance.CloseCurrentWindow();

			if (m_okCallback != null)
			{
				m_okCallback();
			}
			m_okCallback = null;
			m_cancelCallback = null;
		}

		public void ClickCancel()
		{
			UIWindowMgr.Instance.CloseCurrentWindow();

			if (m_cancelCallback != null)
			{
				m_cancelCallback();
			}
			m_okCallback = null;
			m_cancelCallback = null;
		}
	}
}

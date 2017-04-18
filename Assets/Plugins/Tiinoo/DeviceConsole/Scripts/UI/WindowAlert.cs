using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Tiinoo.DeviceConsole
{
	public class WindowAlert : MonoBehaviour 
	{
		public Text uiTitle;
		public Text uiMessage;
		public Text uiOKText;

		// if title, message, ok is null, it means use default value
		public void ShowAlert(string title, string message, string ok)
		{
			uiTitle.text = StringUtil.GetNotNullString(title, "Title");
			uiMessage.text = StringUtil.GetNotNullString(message, "Message");
			uiOKText.text = StringUtil.GetNotNullString(ok, "OK");

			UIWindowMgr.Instance.PopUpWindow(UIWindow.Id.Alert);
		}

		public void ClickOK()
		{
			UIWindowMgr.Instance.CloseCurrentWindow();
		}

	}
}


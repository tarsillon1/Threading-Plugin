using UnityEngine;
using System.Collections;

namespace Tiinoo.DeviceConsole
{
	public class WindowUtil : MonoBehaviour
	{
		public static void ShowAlert(string title, string message, string ok = null)
		{
			WindowAlert alert = UIWindowMgr.Instance.FindWindowComponent(UIWindow.Id.Alert, typeof(WindowAlert)) as WindowAlert;
			alert.ShowAlert(title, message, ok);
		}

		public static void ShowMessageBox(string title, string message, System.Action okCallback = null, System.Action cancelCallback = null)
		{
			ShowMessageBox(title, message, null, null, okCallback, cancelCallback);
		}

		public static void ShowMessageBox(string title, string message, string ok, string cancel, System.Action okCallback, System.Action cancelCallback)
		{
			WindowMessageBox messageBox = UIWindowMgr.Instance.FindWindowComponent(UIWindow.Id.MessageBox, typeof(WindowMessageBox)) as WindowMessageBox;
			messageBox.ShowMessageBox(title, message, ok, cancel, okCallback, cancelCallback);
		}
	}
}

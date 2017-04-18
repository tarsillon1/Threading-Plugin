using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Tiinoo.DeviceConsole
{
	public class DC : MonoBehaviour 
	{
		void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		void Start()
		{
			UIWindowMgr.Instance.Init(DCSettings.Instance);
			UIWindowMgr.Instance.PopUpWindow(UIWindow.Id.Detector);
		}
	}
}

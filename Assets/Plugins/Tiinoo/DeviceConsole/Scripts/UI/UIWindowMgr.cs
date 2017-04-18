using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Tiinoo.DeviceConsole
{
	public class UIWindowMgr : MonoBehaviour
	{
		#region inspector
		public Canvas canvas;
		public EventSystem eventSystem;
		public UIWindow.Cfg[] cfgs;
		#endregion
		
		private Dictionary<UIWindow.Id, UIWindow.Cfg> m_windowCfgs = new Dictionary<UIWindow.Id, UIWindow.Cfg>();

		[SerializeField]
		private List<UIWindow.Id> m_windowIdList = new List<UIWindow.Id>();

		private RectTransform m_canvasRectTrans;
		
		private static UIWindowMgr m_instance;
		
		public static UIWindowMgr Instance
		{
			get {return m_instance;}
		}
		
		void Awake()
		{
			m_instance = this;
			m_canvasRectTrans = canvas.GetComponent<RectTransform>();
		}
		
		public void Init(DCSettings settings)
		{
			// create window objects
			foreach (UIWindow.Cfg cfg in cfgs)
			{
//				cfg.go = ClonePrefab(cfg.prefab, canvas.transform);
				m_windowCfgs[cfg.id] = cfg;
			}
			
			// active eventSystem if needed
			if (EventSystem.current == null)
			{
				eventSystem.gameObject.SetActive(true);
			}

			canvas.sortingOrder = settings.canvasSortOrder;
		}
		
		private static GameObject ClonePrefab(GameObject prefab, Transform parent)
		{
			GameObject go = GameObject.Instantiate(prefab) as GameObject;
			go.name = prefab.name;
			go.transform.SetParent(parent, false);
			return go;
		}
		
		public void PopUpWindow(UIWindow.Id windowId, bool showPrevWindow = true)
		{
			AddWindow(windowId, showPrevWindow);
		}
		
		public void SwitchWindow(UIWindow.Id windowId)
		{
			CloseCurrentWindow();
			PopUpWindow(windowId);
		}
		
		public void CloseCurrentWindow()
		{
			UIWindow.Id windowId = GetCurrentWindowId();
			CloseWindow(windowId);
		}
		
		public void CloseWindow(UIWindow.Id windowId)
		{
			RemoveWindow(windowId);
		}
		
		public void CloseAllWindows()
		{
			for (int i = Count-1; i >- 0; i--)
			{
				CloseCurrentWindow();
			}
		}
		
		private void AddWindow(UIWindow.Id windowId, bool showPrevWindow)
		{
			UIWindow.Cfg cfg = FindWindowCfg(windowId);
			if (cfg == null)
			{
				return;
			}
			
			// show or hide previous window
			DisplayCurrentWindow(showPrevWindow);
			
			m_windowIdList.Add(cfg.id);
			
			// show current window
			DisplayCurrentWindow(true);
		}
		
		private void RemoveWindow(UIWindow.Id windowId)
		{
			UIWindow.Cfg cfg = FindWindowCfg(windowId);
			if (cfg == null)
			{
				return;
			}
			
			int index = FindWindowIndex(windowId);
			if (index == -1)
			{
				return;
			}
			
			// hide window
			DisplayWindow(cfg, false);
			
			m_windowIdList.RemoveAt(index);
			
			// show current window
			DisplayCurrentWindow(true);
		}
		
		public UIWindow.Cfg FindWindowCfg(UIWindow.Id windowId)
		{
			UIWindow.Cfg cfg = null;
			m_windowCfgs.TryGetValue(windowId, out cfg);
			return cfg;
		}

		public Component FindWindowComponent(UIWindow.Id windowId, System.Type type)
		{
			UIWindow.Cfg cfg = FindWindowCfg(windowId);
			if (cfg == null)
			{
				return null;
			}

			Component component = cfg.go.transform.GetComponent(type);
			return component;
		}
		
		private void DisplayCurrentWindow(bool show)
		{
			UIWindow.Id windowId = GetCurrentWindowId();
			UIWindow.Cfg cfg = FindWindowCfg(windowId);
			DisplayWindow(cfg, show);
		}
		
		private static void DisplayWindow(UIWindow.Cfg cfg, bool show)
		{
			if (cfg == null || cfg.go == null)
			{
				return;
			}
			
			cfg.go.SetActive(show);
		}
		
		public UIWindow.Id GetCurrentWindowId()
		{
			if (Count <= 0)
			{
				return UIWindow.Id.Invalid;
			}
			
			int index = Count - 1;
			return m_windowIdList[index];
		}
		
		private int FindWindowIndex(UIWindow.Id windowId)
		{
			int index = -1;
			for (int i = Count-1; i >= 0; i--)
			{
				if (m_windowIdList[i] == windowId)
				{
					index = i;
					break;
				}
			}
			return index;
		}
		
		public int Count
		{
			get {return m_windowIdList.Count;}
		}

		public float CanvasWidth
		{
			get {return m_canvasRectTrans.sizeDelta.x;}
		}

		public float CanvasHeight
		{
			get {return m_canvasRectTrans.sizeDelta.y;}
		}
	}
}


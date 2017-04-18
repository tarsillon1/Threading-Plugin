using UnityEngine;
using System.Collections;

namespace Tiinoo.DeviceConsole
{
	public class DCLoader : MonoBehaviour
	{
		private static bool m_isLoaded;
		private static GameObject m_objDC;

		void Awake()
		{
			#if DISABLE_DC
			// disable device console
			#else
			Load();
			#endif
			
			Destroy(gameObject);
		}
		
		public static void Load()
		{
			if (m_isLoaded)
			{
				return;
			}

			CreateObjDC();
			RegisterLogCallback();
			m_isLoaded = true;

//			Debug.Log("DCLoader.Load()");
		}

		public static void Unload()
		{
			if (!m_isLoaded)
			{
				return;
			}

			DestroyObjDC();
			UnregisterLogCallback();
			m_isLoaded = false;

//			Debug.Log("DCLoader.Unload()");
		}

		private static void CreateObjDC()
		{
			string pluginName = DCConst.PLUGIN_NAME;
			string prefabPath = DCConst.DC_PREFAB_PATH_IN_RESOURCES;

			GameObject prefab = Resources.Load<GameObject>(prefabPath);
			if (prefab == null)
			{
				Debug.LogError(string.Format("[{0}] error: {1} doesn't exist!", pluginName, prefabPath));
				return;
			}

			m_objDC = Object.Instantiate(prefab) as GameObject;
			m_objDC.name = pluginName;
		}

		private static void DestroyObjDC()
		{
			if (m_objDC != null)
			{
				Destroy(m_objDC);
				m_objDC = null;
			}
		}

		private static void RegisterLogCallback()
		{
			#if (UNITY_5 || UNITY_6)
			Application.logMessageReceived += LogHandler.LogCallback;
			#else
			Application.RegisterLogCallback(LogHandler.LogCallback);
			#endif
		}

		private static void UnregisterLogCallback()
		{
			#if (UNITY_5 || UNITY_6)
			Application.logMessageReceived -= LogHandler.LogCallback;
			#else
			Application.RegisterLogCallback(null);
			#endif
		}

		public static GameObject GetObjDC()
		{
			return m_objDC;
		}
	}
}


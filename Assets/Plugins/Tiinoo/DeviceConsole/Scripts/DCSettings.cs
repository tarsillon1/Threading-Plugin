using UnityEngine;
using System.Collections;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tiinoo.DeviceConsole
{
	public class DCSettings : ScriptableObject
	{
		// open
		public Gesture openWithGesture = Gesture.SWIPE_DOWN_WITH_2_FINGERS;
		public KeyCode openWithKey = KeyCode.F1;
		
		// display
		public int uiLayer = 5;
		public int canvasSortOrder = 100;

		// console
		public bool exceptionNotification = true;
		
		public enum Gesture
		{
			None,
			SWIPE_DOWN_WITH_1_FINGER,
			SWIPE_DOWN_WITH_2_FINGERS,
			SWIPE_DOWN_WITH_3_FINGERS,
		}

		private static DCSettings m_instance;

		public static DCSettings Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = Load();
				}
				return m_instance;
			}
		}

		private static DCSettings Load()
		{
			string pathInResources = DCConst.SETTINGS_ASSET_PATH_IN_RESOURCES;
			string settingsFolder = DCConst.SETTINGS_ASSET_FOLDER;
			string settingsFilename = DCConst.SETTINGS_ASSET_FILENAME;
			
			DCSettings settings = Resources.Load<DCSettings>(pathInResources);
			if (settings == null)
			{
				settings = ScriptableObject.CreateInstance<DCSettings>();
				SaveToAsset(settings, settingsFolder, settingsFilename);
			}

//			Debug.Log(string.Format("[{0}] Load {1}", DCConst.PLUGIN_NAME, settingsFilename));
			return settings;
		}

		private static void SaveToAsset(Object asset, string assetFolder, string assetFilename)
		{
			#if UNITY_EDITOR
			if (!File.Exists(assetFolder))
			{
				Directory.CreateDirectory(assetFolder);
			}
			
			string assetPath = assetFolder + "/" + assetFilename;
			AssetDatabase.CreateAsset(asset, assetPath);
			#endif
		}
	}
}

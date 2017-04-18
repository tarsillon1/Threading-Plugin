using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Tiinoo.DeviceConsole
{
	public class DCEditorWindow : EditorWindow
	{
		private static string[] TAB_TITLES = {"Settings", "About"};

		private static DCSettingsTab m_settingsTab;
		private static DCAboutTab m_aboutTab;
		private int m_currentTab = 0;

		public static DCEditorWindow GetInstance()
		{
			DCEditorWindow window = EditorWindow.GetWindowWithRect<DCEditorWindow>(new Rect(0, 0, 420, 360), true, DCConst.PLUGIN_NAME, true);
			window.Init();
			return window;
		}

		private void Init()
		{
			if (m_settingsTab == null)
			{
				CreateSettingsTab();
			}
			if (m_aboutTab == null)
			{
				CreateAboutTab();
			}
		}

		private void CreateSettingsTab()
		{
			m_settingsTab = new DCSettingsTab();
		}

		private void CreateAboutTab()
		{
			m_aboutTab = new DCAboutTab();
		}
		
		public void ShowSettingsTab()
		{
			m_currentTab = 0;
		}

		public void ShowAboutTab()
		{
			m_currentTab = 1;
		}

		void OnGUI()
		{
			GUILayout.BeginVertical();

			m_currentTab = GUILayout.Toolbar(m_currentTab, TAB_TITLES, GUILayout.Width(320));

			switch (m_currentTab)
			{
			case 0:
				if (m_settingsTab == null)
				{
					CreateSettingsTab();
				}
				m_settingsTab.Draw();
				break;

			case 1:
				if (m_aboutTab == null)
				{
					CreateAboutTab();
				}
				m_aboutTab.Draw();
				break;
			}

			GUILayout.EndVertical(); 
		}
	}
}

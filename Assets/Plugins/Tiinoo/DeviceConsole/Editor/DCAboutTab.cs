using UnityEngine;
using System.Collections;
using UnityEditor;
using Tiinoo.DeviceConsole;

namespace Tiinoo.DeviceConsole
{
	public class DCAboutTab
	{
		public DCAboutTab()
		{

		}

		public void Draw()
		{
			GUILayout.BeginVertical(DCStyles.box_noPadding, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			GUILayout.BeginVertical();
			GUILayout.Space(50);

			string pluginName = DCStyles.Sized(DCConst.PLUGIN_NAME, 20);
			GUILayout.Label(pluginName, DCStyles.label_rich_middleCenter);
			GUILayout.Space(4);

			string pluginVersion = DCStyles.Bold(DCConst.PLUGIN_VERSION);
			GUILayout.Label(pluginVersion, DCStyles.label_rich_middleCenter);
			GUILayout.Space(12);

			if (GUILayout.Button("Website"))
			{
				Application.OpenURL(DCConst.URL_WEBSITE);
			}
			GUILayout.Space(10);

			if (GUILayout.Button("Asset Store Page"))
			{
				Application.OpenURL(DCConst.URL_AS_PLUGIN);
			}
			GUILayout.Space(10);

			if (GUILayout.Button("Other Plugins"))
			{
				Application.OpenURL(DCConst.URL_AS_PUBLISHER);
			}
			GUILayout.Space(6);

			EditorGUILayout.SelectableLabel("Email: " + DCConst.SUPPORT_EMAIL, DCStyles.label_rich_middleCenter);
			
			GUILayout.EndVertical();
			
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();
		}
	}

}


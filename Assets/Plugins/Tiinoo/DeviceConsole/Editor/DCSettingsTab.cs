using UnityEngine;
using System.Collections;
using UnityEditor;
using Tiinoo.DeviceConsole;

namespace Tiinoo.DeviceConsole
{
	public class DCSettingsTab
	{
		public DCSettingsTab()
		{

		}

		public void Draw()
		{
			GUILayoutOption CONTENT_WIDTH = GUILayout.Width(240);
			DCSettings settings = DCSettings.Instance;

			GUILayout.BeginVertical(DCStyles.box_noPadding, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

			EditorGUI.BeginChangeCheck();

			// Open
			GUILayout.Label("<b>Open</b>", DCStyles.label_rich);

			GUILayout.BeginHorizontal();
			GUIContent contentOpenWithGesture = new GUIContent("Open With Gesture", "Select the gesture to open the console on mobile.");
			EditorGUILayout.PrefixLabel(contentOpenWithGesture);
			settings.openWithGesture = (DCSettings.Gesture)EditorGUILayout.EnumPopup(settings.openWithGesture, CONTENT_WIDTH);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUIContent contentOpenWithKey = new GUIContent("Open With Key", "Select the key to open the console on PC.");
			EditorGUILayout.PrefixLabel(contentOpenWithKey);
			settings.openWithKey = (KeyCode)EditorGUILayout.EnumPopup(settings.openWithKey, CONTENT_WIDTH);
			GUILayout.EndHorizontal();

			GUILayout.Space(8);
			DCStyles.DrawHorizontalLine();
			GUILayout.Space(8);

			// Display
			GUILayout.Label("<b>Display</b>", DCStyles.label_rich);

			GUILayout.BeginHorizontal();
			GUIContent contentLayer = new GUIContent("UI Layer", "The layer of the Device Console UI.");
			EditorGUILayout.PrefixLabel(contentLayer);
			settings.uiLayer = EditorGUILayout.LayerField(settings.uiLayer, CONTENT_WIDTH);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUIContent contentCanvasSortOrder = new GUIContent("Canvas Sort Order", "The sort order of the Device Console canvas in Overlay render mode. If you can't make the Device Console appear over your UI, which is made with UGUI and uses Overlay render mode too, please set this value to a bigger value.");
			EditorGUILayout.PrefixLabel(contentCanvasSortOrder);
			settings.canvasSortOrder = EditorGUILayout.IntField(settings.canvasSortOrder, CONTENT_WIDTH);
			GUILayout.EndHorizontal();

			GUILayout.Space(8);
			DCStyles.DrawHorizontalLine();
			GUILayout.Space(8);

			// Console
			GUILayout.Label("<b>Console</b>", DCStyles.label_rich);
			GUIContent contentExceptionNotification = new GUIContent("Exception Notification", "Show the console when an exception occurs even if the console is closed.");
			settings.exceptionNotification = EditorGUILayout.Toggle(contentExceptionNotification, settings.exceptionNotification);

			GUILayout.Space(8);
			DCStyles.DrawHorizontalLine();
			GUILayout.Space(8);

			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(settings);
			}

			GUILayout.EndVertical();
		}
	}

}

using UnityEngine;

namespace Tiinoo.DeviceConsole
{
	public class DCStyles
	{
		public static GUIStyle label_rich;
		public static GUIStyle label_rich_wordWrap;
		public static GUIStyle label_rich_middleCenter;
		public static GUIStyle box_noPadding;
		public static GUIStyle line;

		private static bool m_isInited;

		static DCStyles()
		{
			Init();
		}

		private static void Init()
		{
			if (m_isInited)
			{
				return;
			}

			label_rich = new GUIStyle(GUI.skin.label);
			label_rich.richText = true;

			label_rich_wordWrap = new GUIStyle(label_rich);
			label_rich_wordWrap.wordWrap = true;

			label_rich_middleCenter = new GUIStyle(label_rich);
			label_rich_middleCenter.alignment = TextAnchor.MiddleCenter;

			box_noPadding = new GUIStyle(GUI.skin.box);
			box_noPadding.padding = new RectOffset(0, 0, 0, 0);

			line = new GUIStyle(GUI.skin.box);
			line.border = new RectOffset(1, 1, 1, 1);

			m_isInited = true;
		}

		public static string Bold(string s)
		{
			return string.Format("<b>{0}</b>", s);
		}
		
		public static string Sized(string s, int size)
		{
			return string.Format ("<size={1}>{0}</size>", s, size);
		}

		public static void DrawHorizontalLine(float lineHeight = 1)
		{
			GUILayout.Box(GUIContent.none, line, GUILayout.ExpandWidth(true), GUILayout.Height(lineHeight));
		}
		
		public static void DrawVerticalLine(float lineWidth = 1)
		{
			GUILayout.Box(GUIContent.none, line, GUILayout.ExpandHeight(true), GUILayout.Width(lineWidth));
		}
	}
}
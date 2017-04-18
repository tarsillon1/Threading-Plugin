using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Tiinoo.DeviceConsole
{
	public class UGUIUtil
	{
		public static void ScrollToTop(ScrollRect verticalScrollRect)
		{
			verticalScrollRect.verticalNormalizedPosition = 1f;
		}
		
		public static void ScrollToBottom(ScrollRect verticalScrollRect)
		{
			verticalScrollRect.verticalNormalizedPosition = 0f;
		}
		
		public static void ScrollToLeft(ScrollRect horizontalScrollRect)
		{
			horizontalScrollRect.horizontalNormalizedPosition = 0f;
		}
		
		public static void ScrollToRight(ScrollRect horizontalScrollRect)
		{
			horizontalScrollRect.horizontalNormalizedPosition = 1f;
		}

		public static void ScrollToLeftTop(ScrollRect scrollRect)
		{
			scrollRect.normalizedPosition = new Vector2(0f, 1f);
		}

		public static Rect GetRect(Transform t)
		{
			RectTransform rt = t as RectTransform;
			return rt.rect;
		}
	}
}



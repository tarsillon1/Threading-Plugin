using UnityEngine;
using System.Collections;

namespace Tiinoo.DeviceConsole
{
	public class TransformUtil
	{
		public static void SetLayerRecursive(Transform t, int layer)
		{
			t.gameObject.layer = layer;
			
			int childCount = t.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = t.GetChild(i);
				SetLayerRecursive(child, layer);
			}	
		}
	}
}


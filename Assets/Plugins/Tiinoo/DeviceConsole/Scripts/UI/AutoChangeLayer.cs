using UnityEngine;
using System.Collections;
using Tiinoo.DeviceConsole;

namespace Tiinoo.DeviceConsole
{
	public class AutoChangeLayer : MonoBehaviour 
	{
		void Start() 
		{
			int layer = DCSettings.Instance.uiLayer;
			if (gameObject.layer != layer)
			{
				TransformUtil.SetLayerRecursive(transform, layer);
			}
		}
	}
}

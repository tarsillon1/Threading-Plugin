using UnityEngine;
using System.Collections;

namespace Tiinoo.DeviceConsole
{
	public class KeyDownDetector : GestureDetector
	{
		public static System.Action<KeyCode> onKeyDown;
		
		private KeyCode m_key;
		
		public KeyDownDetector(KeyCode key)
		{
			m_key = key;
		}
		
		public override void Update()
		{
			if (Input.GetKeyDown(m_key))
			{
				if (onKeyDown != null)
				{
					onKeyDown(m_key);
				}
			}
		}
	}
}
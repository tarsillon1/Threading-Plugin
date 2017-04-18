using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Tiinoo.DeviceConsole
{
	[RequireComponent(typeof(Button))]
	public class KeyButton : MonoBehaviour 
	{
		public KeyCode key;

		private Button m_button;
		
		void Awake() 
		{
			m_button = GetComponent<Button>();
		}
		
		void Update() 
		{
			if (Input.GetKeyDown(key)) 
			{
				HandleKeyDown();
			} 
		}
		
		private void HandleKeyDown() 
		{
			m_button.onClick.Invoke();
		}
	}
}


using UnityEngine;
using System.Collections;

namespace Tiinoo.DeviceConsole
{
	public class SwipeMouseDetector : SwipeDetector
	{
		private int m_button;
		private float m_tolerableDegreeCos;
		
		private Vector2 m_startPos;
		private Vector2 m_currentPos;
		private Vector2 m_offset;
		
		public SwipeMouseDetector(int button = 0)
		{
			m_button = button;
			m_tolerableDegreeCos = CalculateTolerableDegreeCos();
		}
		
		public override void Update()
		{
			if (Input.GetMouseButtonDown(m_button))
			{
				m_startPos = Input.mousePosition;
			}
			
			if (Input.GetMouseButtonUp(m_button))
			{
				m_currentPos = Input.mousePosition;
				m_offset = m_currentPos - m_startPos;
				Swipe swipe = DetectSwipe(m_offset, m_tolerableDegreeCos);
				NotifySwipe(swipe);
			}
		}
	}
}

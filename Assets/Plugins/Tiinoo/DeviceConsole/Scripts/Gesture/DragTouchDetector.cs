using UnityEngine;
using System.Collections;

namespace Tiinoo.DeviceConsole
{
	public class DragTouchDetector : DragDetector
	{
		private Vector2 m_startPos;
		private Vector2 m_currentPos;
		private Vector2 m_offset;
		private Touch m_touch;
		
		public override void Update()
		{
			if (Input.touchCount == 0) 
			{
				return;
			}
			
			m_touch = Input.touches[0];
			
			if (m_touch.phase == TouchPhase.Began)
			{
				m_startPos = Input.mousePosition;
			}
			
			if (m_touch.phase == TouchPhase.Moved)
			{
				m_currentPos = Input.mousePosition;
				m_offset = m_currentPos - m_startPos;
				NotifyOnDrag(m_startPos, m_currentPos, m_offset);
				
				m_startPos = m_currentPos;
			}
		}
	}
}



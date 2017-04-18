using UnityEngine;
using System.Collections;

namespace Tiinoo.DeviceConsole
{
	public class DragMouseDetector : DragDetector
	{
		private Vector2 m_startPos;
		private Vector2 m_currentPos;
		private Vector2 m_offset;
		
		public override void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				m_startPos = Input.mousePosition;
			}
			
			if (Input.GetMouseButton(0))
			{
				m_currentPos = Input.mousePosition;
				m_offset = m_currentPos - m_startPos;
				NotifyOnDrag(m_startPos, m_currentPos, m_offset);
				
				m_startPos = m_currentPos;
			}
		}
	}
}


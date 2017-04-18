using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Tiinoo.DeviceConsole
{
	public class DragToResize : MonoBehaviour, IPointerDownHandler, IDragHandler 
	{
		#region inspector
		public RectTransform target;
		public bool resizeHorizontal = true;
		public bool resizeVertical = true;
		public Vector2 minSize = new Vector2(1, 1);
		public Vector2 maxSize = new Vector2(-1, -1);
		#endregion
		
		private Vector2 m_originalTargetSize;
		private Vector2 m_originalPointerLocalPosition;
		
		void Start() 
		{
		}
		
		public void OnPointerDown(PointerEventData data) 
		{
			m_originalTargetSize = target.sizeDelta;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(target, data.position, data.pressEventCamera, out m_originalPointerLocalPosition);
		}
		
		public void OnDrag(PointerEventData data) 
		{
			if (!resizeHorizontal && !resizeVertical)
			{
				return;
			}
			
			Vector2 pointerLocalPosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(target, data.position, data.pressEventCamera, out pointerLocalPosition);
			
			Vector3 offsetToOriginal = pointerLocalPosition - m_originalPointerLocalPosition;
			Vector2 size = m_originalTargetSize + new Vector2(offsetToOriginal.x, -offsetToOriginal.y);
			
			float x = m_originalTargetSize.x;
			float y = m_originalTargetSize.y;
			if (resizeHorizontal)
			{
				float minX = (minSize.x < 0) ? 0 : minSize.x;
				float maxX = (maxSize.x < 0) ? float.MaxValue : maxSize.x;
				x = Mathf.Clamp(size.x, minX, maxX);
			}
			if (resizeVertical)
			{
				float minY = (minSize.y < 0) ? 0 : minSize.y;
				float maxY = (maxSize.y < 0) ? float.MaxValue : maxSize.y;
				y = Mathf.Clamp(size.y, minY, maxY);
			}
			
			target.sizeDelta = new Vector2(x, y);
		}
	}
}

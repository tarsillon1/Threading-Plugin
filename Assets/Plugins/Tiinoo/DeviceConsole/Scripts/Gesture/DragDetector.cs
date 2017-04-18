using UnityEngine;
using System.Collections;

namespace Tiinoo.DeviceConsole
{
	public abstract class DragDetector : GestureDetector
	{
		public event System.Action<Vector2, Vector2, Vector2> onDrag;	// startPos, endPos, offset
		
		protected void NotifyOnDrag(Vector2 startPos, Vector2 endPos, Vector2 offset)
		{
			if (onDrag != null)
			{
				onDrag(startPos, endPos, offset);
			}
		}
	}
}


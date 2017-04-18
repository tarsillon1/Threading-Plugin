using UnityEngine;
using System.Collections;

namespace Tiinoo.DeviceConsole
{
	public abstract class SwipeDetector : GestureDetector
	{
		public static float MIN_SWIPE_DISTANCE = 120f;
		public static float MAX_TOLERABLE_DEGREE = 30f;
		
		public static System.Action onSwipeLeft;
		public static System.Action onSwipeRight;
		public static System.Action onSwipeUp;
		public static System.Action onSwipeDown;
		
		public enum Swipe
		{
			Invalid = 0,
			Left = 1,
			Right = 2,
			Up = 3,
			Down = 4,
		}
		
		protected static float CalculateTolerableDegreeCos()
		{
			float degree = MAX_TOLERABLE_DEGREE;
			float radius = Mathf.Deg2Rad * degree;
			float cos = Mathf.Cos(radius);
			return cos;
		}
		
		protected static Swipe DetectSwipe(Vector2 offset, float tolerableDegreeCos)
		{
			float magnitude = offset.magnitude;
			if (magnitude < MIN_SWIPE_DISTANCE)
			{
				return Swipe.Invalid;
			}
			
			Swipe swipe = Swipe.Invalid;

			Vector2 left = new Vector2(-1f, 0f);
			Vector2 right = new Vector2(1f, 0f);
			Vector2 up = new Vector2(0f, 1f);
			Vector2 down = new Vector2(0f, -1f);
			Vector2 normalized = offset.normalized;

			if (Mathf.Abs(offset.x) >= Mathf.Abs(offset.y))
			{
				if (Vector2.Dot(normalized, left) > tolerableDegreeCos)
				{
					swipe = Swipe.Left;
				}
				else if (Vector2.Dot(normalized, right) > tolerableDegreeCos)
				{
					swipe = Swipe.Right;
				}
			}
			else
			{
				if (Vector2.Dot(normalized, up) > tolerableDegreeCos)
				{
					swipe = Swipe.Up;
				}
				else if (Vector2.Dot(normalized, down) > tolerableDegreeCos)
				{
					swipe = Swipe.Down;
				}
			}
			
			return swipe;
		}
		
		protected static void NotifySwipe(Swipe swipe)
		{
			switch (swipe)
			{
			case Swipe.Left:
				if (onSwipeLeft != null)
				{
					onSwipeLeft();
				}
				break;
				
			case Swipe.Right:
				if (onSwipeRight != null)
				{
					onSwipeRight();
				}
				break;
				
			case Swipe.Up:
				if (onSwipeUp != null)
				{
					onSwipeUp();
				}
				break;
				
			case Swipe.Down:
				if (onSwipeDown != null)
				{
					onSwipeDown();
				}
				break;
			}
		}
	}
}
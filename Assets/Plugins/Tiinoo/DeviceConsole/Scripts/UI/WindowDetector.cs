using UnityEngine;
using System.Collections;
using Tiinoo.DeviceConsole;

namespace Tiinoo.DeviceConsole
{
	public class WindowDetector : MonoBehaviour 
	{
		public GestureDetector m_swipeDetector;
		public GestureDetector m_keyDownDetector;
		
		void Start() 
		{
			LogHandler.onExceptionOccur += HandleOnExceptionOccur;
			CreateDetectors();
		}
		
		void OnDestroy() 
		{
			LogHandler.onExceptionOccur -= HandleOnExceptionOccur;
		}
		
		void OnEnable()
		{
			SwipeDetector.onSwipeDown += HandleOnSwipeDown;
			KeyDownDetector.onKeyDown += HandleOnKeyDown;
		}
		
		void OnDisable()
		{
			SwipeDetector.onSwipeDown -= HandleOnSwipeDown;
			KeyDownDetector.onKeyDown -= HandleOnKeyDown;
		}
		
		void Update()
		{
			DetectInput();
		}
		
		private void CreateDetectors()
		{
			DCSettings settings = DCSettings.Instance;
			
			DCSettings.Gesture openWithTouch = settings.openWithGesture;
			switch (openWithTouch)
			{
			case DCSettings.Gesture.None:
				break;
				
			case DCSettings.Gesture.SWIPE_DOWN_WITH_1_FINGER:
				m_swipeDetector = new SwipeTouchDetector(1);
				break;
				
			case DCSettings.Gesture.SWIPE_DOWN_WITH_2_FINGERS:
				m_swipeDetector = new SwipeTouchDetector(2);
				break;
				
			case DCSettings.Gesture.SWIPE_DOWN_WITH_3_FINGERS:
				m_swipeDetector = new SwipeTouchDetector(3);
				break;
			}
			
			KeyCode key = settings.openWithKey;
			m_keyDownDetector = new KeyDownDetector(key);
		}
		
		private void DetectInput()
		{
			if (m_swipeDetector != null)
			{
				m_swipeDetector.Update();
			}
			
			if (m_keyDownDetector != null)
			{
				m_keyDownDetector.Update();
			}
		}
		
		private void HandleOnSwipeDown()
		{
			ShowWindowConsole();
		}
		
		private void HandleOnKeyDown(KeyCode key)
		{
			ShowWindowConsole();
		}

		private void HandleOnExceptionOccur()
		{
			DCSettings settings = DCSettings.Instance;
			if (settings.exceptionNotification)
			{
				ShowWindowConsole();
			}
		}
		
		private void ShowWindowConsole()
		{
			if (WindowConsole.isVisible)
			{
				return;
			}
			
			UIWindowMgr.Instance.PopUpWindow(UIWindow.Id.Console, false);
		}
	}
}

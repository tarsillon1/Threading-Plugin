using UnityEngine;
using System.Collections;

namespace Tiinoo.DeviceConsole
{
	public class PlatformUtil
	{
		public static bool IsMobilePlatform()
		{
			if (SystemInfo.deviceType == DeviceType.Handheld)
			{
				return true;
			}
			return false;
		}
	}
}


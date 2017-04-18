using UnityEngine;
using System.Collections;
using System;

namespace Tiinoo.DeviceConsole
{
	public class TimeUtil
	{
		private static DateTime m_beginTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		
		public static long CurrentTimeStamp(bool useMilliseconds = true)
		{
			long timeStamp = 0;
			
			TimeSpan timeSpan = DateTime.UtcNow - m_beginTime;
			if (useMilliseconds)
			{
				timeStamp = Convert.ToInt64(timeSpan.TotalMilliseconds);
			}
			else
			{
				timeStamp = Convert.ToInt64(timeSpan.TotalSeconds);
			}
			
			return timeStamp;
		}
		
		// convert to the date format of photos in dcim
		// such as: 20160715_090642
		public static string ToDCIMDateFormat(DateTime dateTime)
		{
			string format = dateTime.ToString("yyyyMMdd_HHmmss");
			return format;
		}
	}
}



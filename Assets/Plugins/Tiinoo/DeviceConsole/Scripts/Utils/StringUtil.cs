using UnityEngine;
using System.Collections;

namespace Tiinoo.DeviceConsole
{
	public class StringUtil
	{
		// If str is null, just return defaultStr.
		// Otherwise return str itself.
		public static string GetNotNullString(string str, string defaultStr = "")
		{
			if (str == null)
			{
				return defaultStr;
			}
			return str;
		}
	}
}


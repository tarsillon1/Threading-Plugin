using UnityEngine;
using System.Collections;
using System;

namespace Tiinoo.DeviceConsole
{
	public class DCWatcher
	{
		private static string STR_FLAG = "[Watch]";
		
		public static void Log(string message)
		{
			LogImpl(LogType.Log, message, null);
		}
		
		public static void Log(string message, UnityEngine.Object context)
		{
			LogImpl(LogType.Log, message, context);
		}
		
		public static void LogWarning(string message)
		{
			LogImpl(LogType.Warning, message, null);
		}
		
		public static void LogWarning(string message, UnityEngine.Object context)
		{
			LogImpl(LogType.Warning, message, context);
		}
		
		public static void LogError(string message)
		{
			LogImpl(LogType.Error, message, null);
		}
		
		public static void LogError(string message, UnityEngine.Object context)
		{
			LogImpl(LogType.Error, message, context);
		}

		// because we can't add flag to exception, so give up these methods.
//		public static void LogException(Exception e)
//		{
//			LogException(e, null);
//		}
//		
//		public static void LogException(Exception e, UnityEngine.Object context)
//		{
//			string message = STR_FLAG + e.ToString();
//			Debug.LogException(e, context);
//		}

		private static void LogImpl(LogType logType, string message, UnityEngine.Object context)
		{
			if (message == null)
			{
				message = "";
			}
			
			message = STR_FLAG + message;
			
			switch (logType)
			{
			case LogType.Log:
				Debug.Log(message, context); 
				break;
				
			case LogType.Warning:
				Debug.LogWarning(message, context);
				break;

			case LogType.Error:
			case LogType.Exception:
			case LogType.Assert:
				Debug.LogError(message, context);
				break;

			default:
				Debug.Log(message, context); 
				break;
			}
		}
		
		public static bool HasWatchFlag(string message)
		{
			if (message.StartsWith(STR_FLAG))
			{
				return true;
			}
			return false;
		}

		public static string RemoveWatchFlagIfHas(string message, ref bool hasWatchFlag)
		{
			hasWatchFlag = HasWatchFlag(message);
			string s = message;
			if (hasWatchFlag)
			{
				s = message.Substring(STR_FLAG.Length);
			}
			return s;
		}
	}
}

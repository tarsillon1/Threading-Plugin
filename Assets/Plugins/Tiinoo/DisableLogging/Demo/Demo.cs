using UnityEngine;
using System.Collections;

namespace Tiinoo.DisableLogging
{
	[ExecuteInEditMode]
	public class Demo : MonoBehaviour 
	{
		public bool log = false;

		void Start()
		{
			// if you don't like using DLogOption.cs to set DLog option, 
			// just use the following codes to set it.
//			DLog.enableLogInfo = false;		// This variable controls "DLog.Log()".
//			DLog.enableLogWarning = true;	// This variable controls "DLog.LogWarning()".
//			DLog.enableLogError = true;		// This variable controls "DLog.LogError()" and "DLog.LogException()".
		}
		
		void Update()
		{
			if (log)
			{
				DoLogs(gameObject);
				log = false;
			}
		}
		
		public static void DoLogs(Object context) 
		{
			// Examples of DLog.Log()
			DLog.Log("Log a message");
			DLog.Log("Log a message with a specific color", Color.blue);
			DLog.Log("Log a message with a user-defined color", new Color(0f, 0.5f, 0f));

			// Examples of DLog.LogWarning()
			DLog.LogWarning("Log a warning message");
			DLog.LogWarning("Log a warning message with a specific color", Color.yellow);
			DLog.LogWarning("Log a warning message with a user-defined color", new Color(1f, 0.5f, 0.25f));

			// Examples of DLog.LogError()
			DLog.LogError("Log an error message");
			DLog.LogError("Log an error message with a specific color", Color.red);
			DLog.LogError("Log an error message with a user-defined color", new Color(1f, 0f, 1f));

			// Examples of DLog.LogException()
			DLog.LogException(new System.Exception("Log an exception message"));
			DLog.LogException(new System.Exception("Log an exception message with a specific color"), Color.red);
			DLog.LogException(new System.Exception("Log an exception message with a user-defined color"), new Color(1f, 0f, 1f));

			// Examples of logging With Context
			DLog.Log("Log a message with a context", context);
			DLog.Log("Log a message with a specific color and context", Color.green, context);

			DLog.LogWarning("Log a warning message with a context", context);
			DLog.LogWarning("Log a warning message with a specific color and context", Color.cyan, context);

			DLog.LogError("Log an error message with a context", context);
			DLog.LogError("Log an error message with a specific color and context", Color.red, context);

			DLog.LogException(new System.Exception("Log an exception message with a context"), context);
			DLog.LogException(new System.Exception("Log an exception message with a specific color and context"), Color.red, context);
		}
	}
}



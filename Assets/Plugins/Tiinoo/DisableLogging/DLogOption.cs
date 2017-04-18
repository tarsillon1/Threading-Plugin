using UnityEngine;
using System.Collections;

namespace Tiinoo.DisableLogging
{
	[ExecuteInEditMode]
	public class DLogOption : MonoBehaviour
	{
		public bool logInfo = false;
		public bool logWarning = true;
		public bool logError = true;
		public bool enableAllForEditorMode = true;
		public bool enableAllForDevelopmentBuild = true;

		void Awake()
		{
			if (Application.isPlaying)
			{
				DontDestroyOnLoad(gameObject);
			}
		}

		void Start()
		{
			DoApply();
		}

		void Update()
		{
			#if UNITY_EDITOR
			DoApply();
			#endif
		}

		void DoApply()
		{
			if (enableAllForEditorMode && IsEditorMode())
			{
				EnableAllLogs();
				return;
			}

			if (enableAllForDevelopmentBuild && IsDevelopmentBuild())
			{
				EnableAllLogs();
				return;
			}

			EnableLogsWithOptions();
		}

		private void EnableLogsWithOptions()
		{
			DLog.enableLogInfo = logInfo;
			DLog.enableLogWarning = logWarning;
			DLog.enableLogError = logError;
		}
		
		private void EnableAllLogs()
		{
			DLog.enableLogInfo = true;
			DLog.enableLogWarning = true;
			DLog.enableLogError = true;
		}

		private static bool IsDevelopmentBuild()
		{
			#if UNITY_EDITOR
				return false;
			#else
				return Debug.isDebugBuild;
			#endif
		}

		private static bool IsEditorMode()
		{
			#if UNITY_EDITOR
				return true;
			#else
				return false;
			#endif
		}
	}
}

Disable Logging

Online Docs: http://www.tiinoo.com/disable-logging-docs

--------------------------------------------------
1. Overview
--------------------------------------------------
Disable Logging 
- disable logs in builds with 1-Click 

Is this a tool for you?
- Does it seem like a huge task to remove all calls to Debug.Log in release builds?
- Are you trying to use "Debug.isDebugBuild" or "Scripting Define Symbols" to check for debug?
- Do you want to output specific logs with color to stand out from the rest logs?
If you have met any of the above issues, this is the tool for you.

Features:
- Enable/Disable logs in builds easily.
- Outputs the specific logs with the color you like.

--------------------------------------------------
2. Quick Start
--------------------------------------------------
Before you begin the upgrade, don't forget to remove the old version of this plugin. (See the section "How to remove the plugin?")

Import the plugin, then read the following instructions.

--------------------------------------------------
2.1 How to enable/disable logs in builds?
--------------------------------------------------
(1) Call DLog.Log() instead of Debug.Log().
(2) DLog.Log() is in the UnityEngine namespace, so add "using UnityEngine;" to the scripts which call the DLog.Log() if needed.
(3) Attach the DLogOption.cs to a game object.
(4) Set the options of DLogOption component.

Options of DLogOption component:
- Log Info
This option can enable/disable "DLog.Log()"

- Log Warning
This option can enable/disable "DLog.LogWarning()"

- Log Error
This option can enable/disable "DLog.LogError()" and "DLog.LogException()"

- Enable All For Editor Mode
If you check this option, all the logs (info, warning, error, exception) will be outputted in the unity editor mode. Ignore settings of "Log Info", "Log Warning" and "Log Error".

- Enable All For Development Build
If you check this option, and set the build settings as a "Development Build", then all the logs (info, warning, error, exception) will be outputted in the development build. Ignore settings of "Log Info", "Log Warning" and "Log Error".

But, how to set the build settings as a "Development Build"? 
Unity menu, File > Build Settings..., check the option "Development Build". That's all.

--------------------------------------------------
2.2 How to output colored logs?
--------------------------------------------------
This plugin provides many simple APIs, which can help you output the colored logs easily.

Example codes:
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

--------------------------------------------------
2.3 Is there a demo?
--------------------------------------------------
(1) Open the Demo.unity, which is at: Assets\Tiinoo\DisableLogging\Demo\Demo.unity
(2) Find the Demo object in the hierarchy.
(3) Change the options of the component DLogOption(Script).
(4) Click the Log checkbox of the component Demo(Script). Then you will see the logs outputted in the unity console.
 
--------------------------------------------------
2.4 How to upgrade from the old version?
--------------------------------------------------
If you have used an old version of the plugin (version <= 2.0.0) in your project and want to upgrade it to the newest version of the plugin (version >= 2.1.0), please do the following steps:
(1) Remove the old version of the plugin.
(2) Attach DLogOption.cs to a game object. (Instead of LoggerOption.cs)
(3) Replace Logger with DLog in your codes.
(4) Delete using DisableLogging; in your codes.
Tips: 
We can use the menu item in MonoDevelop to do the Step (3) and step (4). 
(In MonoDevelop, use the menu item: Search > Replace in Files... )

--------------------------------------------------
2.5 How to remove the plugin?
--------------------------------------------------
To remove the plugin (version <= 2.0.0)
(1) Delete the folder: Assets/DisableLogging
(2) Delete the file: Assets/Plugins/DisableLogging.dll

To remove the plugin (version >= 2.1.0)
Delete the folder: Assets/Tiinoo/DisableLogging

--------------------------------------------------
3. Support
--------------------------------------------------
If you have any questions or suggestions, please send an email to me.

Email: support@tiinoo.com
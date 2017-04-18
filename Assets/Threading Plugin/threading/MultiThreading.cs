using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using Priority_Queue;
using System.Reflection;
using System.ComponentModel;
using UnityEngine;


public class MultiThreading : Update
{

	public static bool mainIsRunning;

	private static Dictionary<int, TaskThread> activeThreads = new Dictionary<int, TaskThread> ();
	private static int currentThreadID = 0;
	private static int currentTaskID = 0;
	private static Queue<Task> mainThreadTasks = new Queue<Task>();
	private static Dictionary<Task, Action> onCompletionDic = new Dictionary<Task, Action>();

	/// <summary>
	/// Initializes the static <see cref="MultiThreading"/> class.
	/// Start seperate abort thread, to handle when the application stops playing or is shutdown.
	/// Create MultiThreading obj of type static update, to act as main unity thread.
	/// </summary>
	static MultiThreading ()
	{
		mainIsRunning = true;

		new MultiThreading ();

		Thread abortThread = new Thread (() => {

			try{
				bool running = true;
				while (running) {

					if (!mainIsRunning) {

						stopAll ();
						Thread.CurrentThread.Abort ();
						running = false;
						
					}

					Thread.Sleep (1000);
				}
			}catch(ThreadAbortException e){
				DLog.Log ("ABORTED THREAD ENDED", UnityEngine.Color.yellow);
			}

		}, 131072);

		abortThread.IsBackground = true;
		abortThread.Start ();

	}

	public MultiThreading () : base(){}

	/// <summary>
	/// Raises the update event.
	/// Checks if main thread is still running.
	/// Checks if there are any tasks that need to be completed on the main thread.
	/// </summary>
	public override void OnUpdate ()
	{
		
		mainIsRunning = UnityEngine.Application.isPlaying;
		if (Thread.CurrentThread.ThreadState == ThreadState.Aborted)
			mainIsRunning = false;

		if (mainThreadTasks.Count > 0) {

			for(int amount = 0; amount < mainThreadTasks.Count; amount++)
				mainThreadTasks.Dequeue ().action();

		}

	}

	/// <summary>
	/// Raises the application quit event.
	/// On quit, signal to threads that game has been quit.
	/// </summary>
	public override void OnApplicationQuit ()
	{
		mainIsRunning = false;
	}

	/// <summary>
	/// Starts the new thread of with given memory amount.
	/// </summary>
	/// <returns>The new thread id.</returns>
	/// <param name="memory">Memory.</param>
	public static int startNewThread (int memory)
	{
			
		TaskThread newThread = new TaskThread (currentThreadID, memory);
		newThread.thread.IsBackground = true;
		newThread.Start ();
		activeThreads.Add (currentThreadID, newThread);
	
		currentThreadID += 1;
		return currentThreadID - 1;

		return -1;
	}

	/// <summary>
	/// Checks if other threads are idle.
	/// </summary>
	/// <returns><c>true</c>, if other threads are idle, <c>false</c> otherwise.</returns>
	/// <param name="current">Current thread id.</param>
	public static bool threadsIdle (int current)
	{
		bool idle = false;
		foreach (TaskThread thread in MultiThreading.activeThreads.Values) {
			if (thread.tasks.Count != 0 && thread != activeThreads [current]) {
				idle = true;
				break;
			}
		}

		return idle;
	}

	/// <summary>
	/// Load balances the task. (Assigns task to thread that is doing the least work)
	/// </summary>
	/// <returns>The balanced task.</returns>
	/// <param name="action">The action to perform.</param>
	public static Task loadBalanceTask (Action action)
	{
		SimplePriorityQueue<TaskThread> queue = new SimplePriorityQueue<TaskThread> ();
		foreach (TaskThread thread in activeThreads.Values) {

			queue.Enqueue (thread, thread.tasks.Count);

		}

		TaskThread t = queue.Dequeue ();
		Task task = new Task(action, currentTaskID, t.id);
		task.debugTask = false;
		t.tasks.Enqueue (task);
		currentTaskID += 1;

		return task;
	}

	public static Task loadBalanceTask (Action action, bool debugTask)
	{
		SimplePriorityQueue<TaskThread> queue = new SimplePriorityQueue<TaskThread> ();
		foreach (TaskThread thread in activeThreads.Values) {

			queue.Enqueue (thread, thread.tasks.Count);

		}

		TaskThread t = queue.Dequeue ();
		Task task = new Task(action, currentTaskID, t.id);
		task.debugTask = true;
		t.tasks.Enqueue (task);

		if (debugTask) {
			DLog.Log ("NEW TASK " + currentTaskID + " FOR:" + t.id, UnityEngine.Color.yellow);
		}

		currentTaskID += 1;

		return task;
	}

	/// <summary>
	/// Does the task on given thread id.
	/// </summary>
	/// <returns>The task.</returns>
	/// <param name="threadID">Thread ID.</param>
	/// <param name="action">The action to perform.</param>
	public static Task doTask (int threadID, Action action)
	{
		Task task = new Task(action, currentTaskID, threadID);
		task.debugTask = false;
		activeThreads [threadID].tasks.Enqueue (task);
		currentTaskID += 1;

		return task;
	}

	public static Task doTask (int threadID, Action action, bool debugTask)
	{
		Task task = new Task(action, currentTaskID, threadID);
		task.debugTask = debugTask;
		activeThreads [threadID].tasks.Enqueue (task);

		if (debugTask) {
			DLog.Log ("NEW TASK " + currentTaskID + " FOR:" + threadID, UnityEngine.Color.yellow);
		}
		
		currentTaskID += 1;

		return task;
	}

	/// <summary>
	/// Does the task on main thread.
	/// </summary>
	/// <returns>The task on main thread.</returns>
	/// <param name="action">The action to perform.</param>
	public static Task doOnMainThread(Action action){
		Task task = new Task(action, currentTaskID, -1);
		mainThreadTasks.Enqueue(task);
		currentTaskID ++;

		return task;
	}

	/// <summary>
	/// Sets the task completion to do, when the given task is completed.
	/// </summary>
	/// <param name="taskID">The given task.</param>
	/// <param name="action">The action to perform on completion.</param>
	public static void setOnCompletion(Task task, Action action){
		if(!onCompletionDic.ContainsKey(task))
			onCompletionDic.Add(task, action);
		else
			onCompletionDic[task] = action;
	}

	/// <summary>
	/// Does task on completion.
	/// </summary>
	/// <param name="taskID">Completed task.</param>
	public static void onCompletion(Task task){
		if (onCompletionDic.ContainsKey (task)) {
			
			if (task.debugTask) {
				DLog.Log ("ON COMPLETION TRIGGERED FOR: " + task.taskID, UnityEngine.Color.yellow);
			}
			
			onCompletionDic [task] ();
		}
	}

	public static void stopThread (int threadID)
	{
		
		activeThreads [threadID].stop = true;
		activeThreads [threadID].thread.Abort ();
		GC.Collect ();

	}

	/// <summary>
	/// Stops all threads.
	/// </summary>
	public static void stopAll ()
	{

		DLog.Log ("ABORTING ALL THREADS", UnityEngine.Color.yellow);

		foreach (TaskThread taskThread in activeThreads.Values) {
			
			taskThread.stop = true;
			taskThread.thread.Abort ();
			GC.Collect ();
		}

		activeThreads = new Dictionary<int, TaskThread> ();
	}

}

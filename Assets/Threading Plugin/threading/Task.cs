using System.Collections;
using System;

public class Task{
	public Action action {get; set;}
	public object results { get; set; }
	public int threadID {get; set;}
	public int taskID {get; set;}
	public bool debugTask { get; set; }

	public Task(Action action, int taskID, int threadID){
		this.action = action;
		this.taskID = taskID;
		this.threadID = threadID;
		this.debugTask = false;
	}

}

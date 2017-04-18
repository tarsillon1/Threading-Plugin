using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticUpdate : MonoBehaviour{
	public static HashSet<Update> updates = new HashSet<Update> ();

	void Update(){
		foreach (Update update in updates)
			update.OnUpdate ();
	}

	void OnApplicationQuit (){
		foreach (Update update in updates)
			update.OnApplicationQuit ();
	}
}

public abstract class Update{
	public Update(){
		StaticUpdate.updates.Add (this);
	}

	public abstract void OnUpdate ();
	public abstract void OnApplicationQuit ();
	
}
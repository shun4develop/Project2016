using UnityEngine;
using System.Collections;

public class LoadingManager{
	private static LoadingPanel loading;

	public static void run(){
		Debug.Log ("Loading...");
		if (loading != null)
			return;
		GameObject obj = (GameObject)Resources.Load ("Prefabs/Loading");
		Debug.Log ("resources obj => "+obj);
		obj = UnityEngine.Object.Instantiate (obj) as GameObject;
		Debug.Log ("instatantiate obj => "+obj);
		loading = obj.GetComponent<LoadingPanel> ();
		GameObject canvas = GameObject.FindGameObjectWithTag ("Canvas");
		loading.transform.SetParent (canvas.transform,false);
		//loading.run ();
	}
	public static void stop(){
		if (loading != null) {
			Debug.Log ("End Loading");
			loading.stop ();
			loading = null;
		}
	}
}

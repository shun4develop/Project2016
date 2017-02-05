using UnityEngine;
using System.Collections;

public class StatusBarManager : MonoBehaviour {
	public static bool statusBarHidden = false;
	public static void hide(){
		if (Application.platform == RuntimePlatform.Android) {
			
		} else if(Application.platform == RuntimePlatform.IPhonePlayer) {
			IOSPlugin.hideStatusBar ();
		}
		statusBarHidden = true;
	}
	public static void show(){
		if (Application.platform == RuntimePlatform.Android) {

		} else if(Application.platform == RuntimePlatform.IPhonePlayer) {
			IOSPlugin.showStatusBar ();
		}
		statusBarHidden = false;
	}
}

using UnityEngine;
using System.Collections;

public class AuthSceneController : MonoBehaviour {
	public AdjustableAnimationUI[] uiArray;
	// Use this for initialization
	void Start () {
		foreach(AdjustableAnimationUI ui in uiArray){
			ui.animation ();
		}
	}
}

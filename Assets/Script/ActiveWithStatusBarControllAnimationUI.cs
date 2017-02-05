using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AnimationUI))]
public class ActiveWithStatusBarControllAnimationUI : MonoBehaviour {
	public bool isHideStatusbar;
	private AnimationUI ui;
	void Start(){
		ui = GetComponent<AnimationUI> ();
	}
	// Update is called once per frame
	void Update () {
		if (ui.IsActive && (StatusBarManager.statusBarHidden^isHideStatusbar)) {
			if (isHideStatusbar) {
				StatusBarManager.hide ();

				Debug.Log ("hide statusbar");
			} else {
				StatusBarManager.show ();
				Debug.Log ("show statusbar");
			}
		}
	}
}

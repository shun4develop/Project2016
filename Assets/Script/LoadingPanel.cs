using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AnimationUI))]
[RequireComponent(typeof(RectTransform))]
public class LoadingPanel : MonoBehaviour {
	private AnimationUI animUI;
	private RectTransform rect;
	// Use this for initialization
	void Start () {
		animUI = GetComponent<AnimationUI> ();
		rect = GetComponent<RectTransform> ();
		run ();
	}
	// Update is called once per frame
	void Update () {
		this.transform.SetAsLastSibling ();
	}
	public void run(){
		animUI.fadeIn ();
	}
	public void stop(){
		animUI.fadeOut ();
		Destroy (this.gameObject);
	}
}

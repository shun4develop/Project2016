using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AnimationWebView : WebViewObject {
	private float SPEED = 0.3f;
	private iTween.EaseType EASE_TYPE = iTween.EaseType.easeOutExpo;
	private AnimationUI controller;
	void Awake(){
		controller = GameObject.Find ("WebViewControllerPage").GetComponent<AnimationUI>();
		Button b = GameObject.Find ("CloseWebViewButton").GetComponent<Button>();
		b.onClick.AddListener (()=>{
			slideOut();
		});
	}
	public void slideIn(){
		controller.slideIn ("BOTTOM");
		SetMargins (0,Screen.height,0,-Screen.height);
		SetVisibility (true);
		Hashtable table = new Hashtable ();
		table.Add ("from", 0);
		table.Add ("to",Screen.height);
		table.Add ("time",SPEED);
		table.Add ("easeType", EASE_TYPE);
		table.Add ("onupdate","moveWebView");
		table.Add ("oncompletetarget",gameObject);
		table.Add ("oncomplete","slideInComplete");

		iTween.ValueTo (gameObject,table);
	}
	public void slideOut(){
		controller.slideOut ("BOTTOM");
		Hashtable table = new Hashtable ();
		table.Add ("from", Screen.height);
		table.Add ("to",0);
		table.Add ("time",SPEED);
		table.Add ("easeType", EASE_TYPE);
		table.Add ("onupdate","moveWebView");
		table.Add ("oncompletetarget",gameObject);
		table.Add ("oncomplete","slideOutComplete");

		iTween.ValueTo (gameObject,table);
	}
	private void moveWebView(int val){
		SetMargins (0,Screen.height-val + (Screen.height/20),0,-Screen.height+val);
	}
	private void slideInComplete(){
		// code...
	}
	private void slideOutComplete(){
		SetVisibility (false);
		Destroy (this.gameObject);
	}
}

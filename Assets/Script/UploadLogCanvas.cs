using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(AnimationUI))]
public class UploadLogCanvas : MonoBehaviour {

	public Text logText;
	private AnimationUI animUI;

	void Start(){
		animUI = GetComponent<AnimationUI> ();
	}
		
	public void uploadComplete(){
		logText.text = "アップロード完了";
		animUI.popUp ();
	}
	public void uploadFailure(){
		logText.text = "アップロード失敗";
		animUI.popUp ();
	}
}

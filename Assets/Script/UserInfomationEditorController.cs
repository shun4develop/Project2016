using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MyManagers;
using System;
[RequireComponent(typeof(AnimationUI))]
[RequireComponent(typeof(CanvasGroup))]
public class UserInfomationEditorController : MonoBehaviour {
	public Text logText;
	public InputField description;
	public Text display;
	public AnimationUI confirmation;

	void Start(){
		update ();
	}


	private void update(){
		Profile info = SaveDataManager.loadUserInfo ();
		string desc = info.getDesc ();
		description.text = desc;
		display.text = desc;
	}
	public void sendNewInfomation () {
		Profile info = SaveDataManager.loadUserInfo ();
		info.setDesc(description.text);
		if (description.text == display.text) {
			logText.text = "保存しました";
			StartCoroutine (wordDelete(2));
			return;
		}
		Action<string> success = (string text) => {
			logText.text = "保存しました";
			SaveDataManager.saveUserInfo(info);
			update();
			StartCoroutine (wordDelete(2));
		};
		Action failure = ()=>{
			logText.text = "保存に失敗しました";
			Debug.Log("ng");
			info.setDesc(display.text);
			StartCoroutine (wordDelete(2));
		};
		WebManager.instance.updateUserInfomation (success,failure,info);
	}
	public void slideIn(){
		update ();
		GetComponent<AnimationUI> ().slideIn ("RIGHT");
	}
	public void slideOut(){
		if (description.text == display.text) {
			GetComponent<AnimationUI> ().slideOut ("RIGHT");
		} else {
			confirmation.popUp ();
		}
	}
	private IEnumerator wordDelete(int sec){
		yield return new WaitForSeconds (sec);
		logText.text = "";
	}
}

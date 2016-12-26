using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using MyManagers;
public class UserInfoController: MonoBehaviour {
	public InputField description;
	public Text userName;
	public Image icon;
	public Text logText;

	void Start(){
		
	}
	private void fetchUserInfo(){
		Action<string> success = (string msg) => {
			Profile profile = JsonUtility.FromJson<Profile>(msg);
			description.text = profile.getDesc();
			userName.text = profile.getUserName();
			//WebManager.instance.getSNSIcon();
			logText.text = "保存しました";
		};
		Action failure = () => {
			logText.text = "取得に失敗しました";
		};
	}

	public void updateInfomation () {
		Profile info = SaveDataManager.loadUserInfo ();
		info.setDesc(description.text);
		Action<string> success = (string text) => {
			logText.text = "保存しました";
			SaveDataManager.saveUserInfo(info);
		};
		Action failure = ()=>{
			logText.text = "保存できません";
			Debug.Log("ng");
		};
		WebManager.instance.updateUserInfomation (success,failure,info);
	}

}

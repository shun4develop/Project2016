using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using MyManagers;
public class UserInfoController: MonoBehaviour {
	public Text description;
	public Text userName;
	public IconImageViewer icon;
	public Text logText;

	void Start(){
		fetchUserInfo ();
	}
	private void fetchUserInfo(){
		Action<string> success = (string msg) => {
			Profile profile = JsonUtility.FromJson<Profile>(msg);
			description.text = profile.getDesc();
			userName.text = profile.getUserName();
			icon.show(profile.getUserIconDataPath());
			logText.text = "";
		};
		Action failure = () => {
			logText.text = "取得に失敗しました";
		};
		WebManager.instance.getUserInfomation (success,failure);
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

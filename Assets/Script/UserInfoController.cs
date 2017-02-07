using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using MyManagers;
public class UserInfoController: MonoBehaviour {
	public Text description;
	public Text userName;
	public IconImageViewer icon;

	void Start(){
		fetchUserInfo ();
	}
	public void fetchUserInfo(string name){
		Action<string> success = (string msg) => {
			Profile profile = JsonUtility.FromJson<Profile>(msg);
			description.text = profile.getDesc();
			userName.text = profile.getUserName();
			icon.show(profile.getUserIconDataPath());
			Debug.Log(profile.getUserIconDataPath());
		};
		Action failure = () => {
		};
		WebManager.instance.getUserInfomation (success,failure,name);
	}
	private void fetchUserInfo(){
		fetchUserInfo (SaveDataManager.loadUserName());
	}
}

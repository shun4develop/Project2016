using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using MyManagers;
public class UserInfoTester : MonoBehaviour {
	public InputField description;

	public void updateInfomation () {
		SaveDataManager.saveUserInfo(new UserInfomation ("original","amano","","icon"));
		UserInfomation info = SaveDataManager.loadUserInfo ();
		info.setDesc(description.text);
		Action<string> success = (string text) => {
			SaveDataManager.saveUserInfo(info);
		};
		Action failure = ()=>{
			Debug.Log("ng");
		};
		WebManager.instance.updateUserInfomation (success,failure,info);

	}

}

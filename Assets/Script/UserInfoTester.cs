﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using MyManagers;
public class UserInfoTester : MonoBehaviour {
	public InputField description;

	public Text infoText;

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


	public void getUserInfo(){
		Action<string> success = (string text) => {
			infoText.GetComponent<Text>().text = text;
			Debug.Log(text);
		};
		Action failure = ()=>{
			Debug.Log("ng");
		};
		WebManager.instance.getUserInfomation(success, failure);
	}

}

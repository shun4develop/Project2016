using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using MyCommon;
using MyLibrary;
using MyManagers;
public class AutoLogin : MonoBehaviour {
	
	public bool success {get;set;}
	public bool inquiryCompleted{ get; set;}
	void Start(){
		
	}
	public void autoLogin(){
		//自動ログインに必要な情報が保存されているかを調べる
		string user_name = SaveDataManager.loadUserName();
		string token = SaveDataManager.loadToken ();
		if (string.IsNullOrEmpty (user_name) || string.IsNullOrEmpty (token)) {
			Debug.Log("トークンがありません->ログイン画面へ");
			success = false;
			inquiryCompleted = true;
			return;
		}
		//コールバック関数の定義
		Action<Dictionary<string,object>> success_func = (Dictionary<string,object> resp) => { 
			success = true;

			string resp_token = (string)resp["token"];
			Profile info = JsonUtility.FromJson<Profile>((string)resp["user_info"]);

			SaveDataManager.saveToken(resp_token);
			SaveDataManager.saveUserInfo(info);
			SaveDataManager.saveUserName(user_name);

			inquiryCompleted = true;

			return;
		};
		Action failure_func = ()=> {
			success = false;
			logout ();

			inquiryCompleted = true;

			return;
		};

		//自動ログインを試みる
		WebManager.instance.autoLogin(success_func,failure_func);
	}
	public void logout(){
		SaveDataManager.deleteData ();
	}
}

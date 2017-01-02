using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using MyCommon;
using MyLibrary;
using MyManagers;
public class AutoLogin : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		//自動ログインに必要な情報が保存されているかを調べる
		string user_name = SaveDataManager.loadUserName();
		string token = SaveDataManager.loadToken ();
		if (string.IsNullOrEmpty (user_name) || string.IsNullOrEmpty (token)) {
			Debug.Log("トークンがありません->ログイン画面へ");
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Auth");
			return;
		}
			
		//コールバック関数の定義
		Action<Dictionary<string,object>> success_func = (Dictionary<string,object> resp) => {
			
			string resp_token = (string)resp["token"];
			Profile info = JsonUtility.FromJson<Profile>((string)resp["user_info"]);

			SaveDataManager.saveToken(resp_token);
			SaveDataManager.saveUserInfo(info);
			SaveDataManager.saveUserName(user_name);

			UnityEngine.SceneManagement.SceneManager.LoadScene ("Main");
			return;
		};
		Action failure_func = ()=> { 
			logout ();
			//ログイン画面へ
			UnityEngine.SceneManagement.SceneManager.LoadScene("Auth");
			return;
		};

		//自動ログインを試みる
		WebManager.instance.autoLogin(success_func,failure_func);
	}

	public void logout(){
		PlayerPrefs.DeleteKey ("token");
	}
}

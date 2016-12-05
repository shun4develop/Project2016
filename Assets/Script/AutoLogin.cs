using UnityEngine;
using System.Collections;
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
		Action<string> success_func = (string text) => {
			SaveDataManager.saveToken(text);
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

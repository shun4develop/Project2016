using UnityEngine;
using UnityEngine.UI;
using System;
using MyManagers;
using System.Collections.Generic;
public class LoginController : MonoBehaviour {
	public InputField userName;
	public InputField password;
	public Text log;

	public void login(){
		if (userName.text.Length == 0 || password.text.Length == 0) {
			log.text = "ユーザ名とパスワードを入力してください";
		}else if(userName.text.Length < 3 || userName.text.Length > 17){
			log.text = "ユーザ名は3文字以上かつ16文字以下の長さが必要です";
		}else if(password.text.Length < 8 || password.text.Length > 65){
			log.text = "パスワードは8文字以上64文字以下の長さが必要です";
		}else {
			


			//コールバック関数の定義
			Action<Dictionary<string,object>> success_func = (Dictionary<string,object> resp) => { 
				
				string resp_token = (string)resp["token"];
				Profile info = JsonUtility.FromJson<Profile>((string)resp["user_info"]);

				SaveDataManager.saveToken(resp_token);
				SaveDataManager.saveUserInfo(info);
				SaveDataManager.saveUserName(userName.text);
				UnityEngine.SceneManagement.SceneManager.LoadScene ("Map");

			};
			Action failure_func = ()=> {
				log.text = "ユーザ名かパスワードが正しくありません";
			};

			//自動ログインを試みる
			WebManager.instance.login (success_func,failure_func,userName.text,password.text);

		}
	}
}

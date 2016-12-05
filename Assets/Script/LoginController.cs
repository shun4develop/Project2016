using UnityEngine;
using UnityEngine.UI;
using System;
using MyManagers;
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
			Action<string> success_func = (string text) => {
				SaveDataManager.saveUserName(userName.text);
				SaveDataManager.saveToken(text);
				UnityEngine.SceneManagement.SceneManager.LoadScene ("Main");
			};
			Action failure_func = () => {
				log.text = "ユーザ名かパスワードが正しくありません";
			};
			WebManager.instance.login (success_func,failure_func,userName.text,password.text);
		}
	}
}

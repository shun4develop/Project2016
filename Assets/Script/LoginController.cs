using UnityEngine;
using UnityEngine.UI;
using System;
using MyManagers;
public class LoginController : MonoBehaviour {
	public Text user_name;
	public Text password;
	public Text log;
	public void login(){
		if (user_name.text.Length == 0 || password.text.Length == 0) {
			log.text = "ユーザ名とパスワードを入力してください";
		}else if(user_name.text.Length < 3 || user_name.text.Length > 17){
			log.text = "ユーザ名は3文字以上かつ16文字以下の長さが必要です";
		}else if(password.text.Length < 8 || password.text.Length > 65){
			log.text = "パスワードは8文字以上64文字以下の長さが必要です";
		}else {
			Action<string> success_func = (string text) => {
				SaveDataManager.saveUserName(user_name.text);
				SaveDataManager.saveToken(text);
				UnityEngine.SceneManagement.SceneManager.LoadScene ("Main");
			};
			Action failure_func = () => {
				log.text = "ユーザ名かパスワードが正しくありません";
			};
			WebManager.instance.login (success_func,failure_func,user_name.text,password.text);
		}
	}
}

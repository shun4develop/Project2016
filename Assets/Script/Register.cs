using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using MyCommon;
using MyManagers;
using System;
using System.Collections.Generic;
public class Register : MonoBehaviour {
	public InputField user_name;
	public InputField password;
	public Text log;
	public SocialLoginPageController slpc;
	public void register(){
		if (!user_name.GetComponent<CheckValidityOfValueInputField> ().IsChecked) {
			return;
		}

		if (user_name.text.Length == 0 || password.text.Length == 0) {
			log.text = "ユーザ名とパスワードを入力してください";
		}else if(user_name.text.Length < 3 || user_name.text.Length > 17){
			log.text = "ユーザ名は3文字以上かつ16文字以下の長さが必要です";
		}else if(password.text.Length < 8 || password.text.Length > 65){
			log.text = "パスワードは8文字以上64文字以下の長さが必要です";
		}else {
			Action<string> success_func = (string text) => {
				SaveDataManager.saveToken (text);
				SaveDataManager.saveUserName (user_name.text);
				//メイン画面
				UnityEngine.SceneManagement.SceneManager.LoadScene ("_main");
			};
			Action failure_func = () => {
				log.text = "登録できません";
			};

			WebManager.instance.userRegister (success_func,failure_func,user_name.text,password.text);
		}
	}
	public void snsRegister(){
		if (!user_name.GetComponent<CheckValidityOfValueInputField> ().IsChecked) {
			return;
		}

		if (user_name.text.Length == 0) {
			log.text = "ユーザ名を入力してください";
		} else if (user_name.text.Length < 3 || user_name.text.Length > 17) {
			log.text = "ユーザ名は3文字以上かつ16文字以下の長さが必要です";
		} else {
			Action<string> success_func = (string text) => {
				SaveDataManager.saveToken (text);
				SaveDataManager.saveUserName (user_name.text);
				//メイン画面
				UnityEngine.SceneManagement.SceneManager.LoadScene ("Main");
			};
			Action failure_func = () => {
				log.text = "登録できません";
			};
			UserOfSNS user = slpc.getSNSUser ();
			user.name = user_name.text;
			WebManager.instance.socialRegister (success_func, failure_func,user,slpc.getToken());
		}
	}
}

//AppDelegateListener.mmに以下をペースト
////オレオレ証明書だとAppleさんに怒られるので
////接続先の証明書が無効な証明書の場合でもYESを返すことようにする。
////もちろんリジェクトされるのでリリース時は信頼できる証明書を取得すること。
//@implementation NSURLRequest(DataController)
//+ (BOOL)allowsAnyHTTPSCertificateForHost:(NSString *)host
//{
//	return  YES;
//}
//@end
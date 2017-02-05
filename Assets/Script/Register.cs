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
	public InputField passwordAgain;
	public Text log;
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
		}else if(password.text != passwordAgain.text){
			log.text = "パスワードと確認用パスワードが異なっています。";
		}else{
			Action<Dictionary<string,object>> success_func = (Dictionary<string,object> dic) => {
				SaveDataManager.saveToken ((string)dic["token"]);
				SaveDataManager.saveUserInfo(JsonUtility.FromJson<Profile>((string)dic["user_info"]));
				SaveDataManager.saveUserName (user_name.text);
				//メイン画面
				UnityEngine.SceneManagement.SceneManager.LoadScene ("Main");
			};
			Action failure_func = () => {
				log.text = "登録できません";
			};
			WebManager.instance.userRegister (success_func,failure_func,user_name.text,password.text);
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
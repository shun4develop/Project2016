using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using MyManagers;
[RequireComponent(typeof(AnimationUI))]
public class SocialRegisterPage : MonoBehaviour {
	public Image img;
	public InputField user_name;
	public Text log;
	private string token;
	private SNSIconImageViewer iv;
	private UserOfSNS user;

	private bool isSetInfo = false;

	void Start(){
		iv = img.GetComponent<SNSIconImageViewer> ();
	}

	public void showRegisterInfo(Dictionary<string,object> dic){
		string user_json = (string)dic ["user_info"];
		if ((string)dic ["sns_type"] == "twitter") {
			user = MyLibrary.JsonHelper.TwitterUserFromJson (user_json);
		}else if((string)dic["sns_type"] == "facebook"){
			user = MyLibrary.JsonHelper.FacebookUserFromJson (user_json);
		}

		if (user == null)
			return;

		token = (string)dic ["token"];

		if (user.GetType () == typeof(UserOfTwitter)) {
			UserOfTwitter t_user = (UserOfTwitter)user;
			iv.show (t_user.profile_image_url);
		} else if (user.GetType () == typeof(UserOfFacebook)) {
			UserOfFacebook f_user = (UserOfFacebook)user;
			iv.show ("http://graph.facebook.com/" + f_user.id + "/picture");
		}
		user_name.text = user.name;

		isSetInfo = true;
	}
	public void register(){
		if (!user_name.GetComponent<CheckValidityOfValueInputField> ().IsChecked || !isSetInfo) {
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
				Debug.Log("失敗");
				log.text = "登録できません";
			};
			user.name = user_name.text;
			WebManager.instance.socialRegister (success_func, failure_func,user,token);
		}
	}
}

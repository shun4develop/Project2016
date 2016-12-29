using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using MyManagers;
[RequireComponent(typeof(AnimationUI))]
public class SocialLoginPage : MonoBehaviour {
	public Image img;
	public Text user_name;
	private string token;
	private IconImageViewer iv;
	private UserOfSNS user;

	private bool isSetInfo = false;

	void Start(){
		iv = img.GetComponent<IconImageViewer> ();
	}

	public void showLoginInfo(Dictionary<string,object> dic){
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
	public void login(){
		if (!isSetInfo) {
			return;
		}
		Action<string> success_func = (string text) => {
			SaveDataManager.saveToken (text);
			SaveDataManager.saveUserName (user_name.text);
			//メイン画面
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Main");
		};
		Action failure_func = () => {
			Debug.Log("失敗");
		};
		Debug.Log ("###"+token);
		WebManager.instance.socialLogin (success_func, failure_func,user,token);

	}
}

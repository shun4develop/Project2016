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
			Debug.Log (t_user);
			iv.show (t_user.profile_image_url);
		} else if (user.GetType () == typeof(UserOfFacebook)) {
			UserOfFacebook f_user = (UserOfFacebook)user;
			Debug.Log (f_user);
			iv.show ("http://graph.facebook.com/" + f_user.id + "/picture");
		}
		user_name.text = user.name;
		Debug.Log (user_name.text);
		isSetInfo = true;
	}
	public void login(){
		if (!isSetInfo) {
			return;
		}
		//コールバック関数の定義
		Action<Dictionary<string,object>> success_func = (Dictionary<string,object> resp) => { 
			string resp_token = (string)resp["token"];
			Profile info = JsonUtility.FromJson<Profile>((string)resp["user_info"]);

			SaveDataManager.saveToken(resp_token);
			SaveDataManager.saveUserInfo(info);
			SaveDataManager.saveUserName(user_name.text);

			UnityEngine.SceneManagement.SceneManager.LoadScene ("Map");
			return;
		};
		Action failure_func = ()=> {
			this.GetComponent<AnimationUI>().slideOut("RIGHT");
			return;
		};

		WebManager.instance.socialLogin (success_func, failure_func,user,token);

	}
}

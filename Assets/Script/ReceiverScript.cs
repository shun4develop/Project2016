using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using MyManagers;

public class ReceiverScript : MonoBehaviour {
	public GameObject socialLoginPage;
	public Text logText;

	public void TriggerOpenURL(string msg){
		GameObject webView = GameObject.Find ("WebViewObject") as GameObject;
		if (webView != null) {
			webView.GetComponent<WebViewObject> ().SetVisibility (false);
			Destroy (webView);
		}
		msg = MyLibrary.UriStringDecoder.decode (msg);

		if (msg.Length < 5) {
			Debug.Log ("short msg");
			return;
		}
		msg = msg.Substring (msg.IndexOf('/')+1);



		Dictionary<string,object> dic = MiniJSON.Json.Deserialize (msg) as Dictionary<string,object>;

		if ((string)dic ["response_type"] == "social_login") {
			socialLogin (dic);
		}


	}
	private void socialLogin(Dictionary<string,object> dic){
		string user_json = (string)dic ["user_info"];
		UserOfSNS user = null;
		if ((string)dic ["sns_type"] == "twitter") {
			user = MyLibrary.JsonHelper.TwitterUserFromJson (user_json);
		}else if((string)dic["sns_type"] == "facebook"){
			user = MyLibrary.JsonHelper.FacebookUserFromJson (user_json);
		}

		if (user == null)
			return;

		string token = (string)dic ["token"];

		if ((bool)dic ["user_find"]) {
			SaveDataManager.saveToken (token);
			SaveDataManager.saveUserName (user.name);

			UnityEngine.SceneManagement.SceneManager.LoadScene ("_main");
			return;
		} else {
			socialLoginPage.SendMessage ("slideIn", "RIGHT");
			socialLoginPage.SendMessage ("setUser", dic ["user_info"]);
		}
	}
}

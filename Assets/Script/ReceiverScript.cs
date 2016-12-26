using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using MyManagers;

public class ReceiverScript : MonoBehaviour {
	public void TriggerOpenURL(string msg){

		//WebViewObjectが見つかったら
		//非表示にしてオブジェクトを破棄する
		GameObject webView = GameObject.Find ("WebViewObject") as GameObject;
		if (webView != null) {
			webView.GetComponent<WebViewObject> ().SetVisibility (false);
			webView.GetComponent<AnimationWebView> ().slideOut ();
		}
		//URLに乗って情報が返されるのでDecodeする
		msg = MyLibrary.UriStringDecoder.decode (msg);
		//メッセージが短すぎると弾きます。せめてレスポンスタイプの指定をしてから返してね。
		if (msg.Length < "response_type".Length) {
			Debug.Log ("short msg");
			return;
		}
		msg = msg.Substring (msg.IndexOf('/')+1);

		Debug.Log (msg);

		Dictionary<string,object> resp = MiniJSON.Json.Deserialize (msg) as Dictionary<string,object>;

		if (resp == null)
			return;
		

		if ((string)resp ["response_type"] == "social_login") {
			socialLogin (resp);
		}

	}
	private void socialLogin(Dictionary<string,object> dic){
		GameObject.Find("SocialLoginController").SendMessage ("socialLogin",dic);
	}
}

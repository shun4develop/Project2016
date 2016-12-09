using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using MyManagers;

public class ReceiverScript : MonoBehaviour {
	public GameObject socialLoginPage;

	public void TriggerOpenURL(string msg){
		//WebViewObjectが見つかったら
		//非表示にしてオブジェクトを破棄する
		GameObject webView = GameObject.Find ("WebViewObject") as GameObject;
		if (webView != null) {
			webView.GetComponent<WebViewObject> ().SetVisibility (false);
			Destroy (webView);
		}
		//URLに乗って情報が返されるのでパースする
		msg = MyLibrary.UriStringDecoder.decode (msg);

		if (msg.Length < 5) {
			Debug.Log ("short msg");
			return;
		}
		msg = msg.Substring (msg.IndexOf('/')+1);



		Dictionary<string,object> dic = MiniJSON.Json.Deserialize (msg) as Dictionary<string,object>;

		if (dic == null)
			return;
		

		if ((string)dic ["response_type"] == "social_login") {
			socialLogin (dic);
		}


	}
	private void socialLogin(Dictionary<string,object> dic){
		socialLoginPage.SendMessage ("login",dic);
	}
}

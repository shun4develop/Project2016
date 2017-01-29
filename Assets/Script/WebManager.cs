using UnityEngine;
using System.Collections;
using System;
using MyManagers;
using UnityEngine.UI;
using MyClass;
using System.Collections.Generic;
public class WebManager : MonoBehaviour {
	private static string DOMAIN = "http://160.16.216.204/~hosoya/puts/";
	//コンテンツデータのリストをもらう
	private string FETCH_CONTENTS = DOMAIN + "fetch_contents.php";
	//自動ログインする
	private string AUTO_LOGIN = DOMAIN + "auto_login.php";
	//持っているコンテンツデータを手元から消す
	private string CONTENTS_DUMP = DOMAIN + "contents_dump.php";
	//道に落ちているコンテンツデータを拾う
	private string CONTENTS_TAKEN = DOMAIN + "contents_taken.php";
	//ユーザ名が被っていないかを調べる
	private string FIND_USER_NAME = DOMAIN + "find_user_name.php";
	//filepathを指定してコンテンツなどをもらう
	private string GET_RESOURCES = DOMAIN + "get_resources.php";
	//ログインする
	private string LOGIN = DOMAIN + "login.php";
	//ソーシャルログイン
	private string SOCIAL_LOGIN_FROM_TWITTER = DOMAIN + "social_login_from_twitter.php";
	private string SOCIAL_LIGIN_FROM_FACEBOOK = DOMAIN + "social_login_from_facebook.php";
	//会員登録する
	private string USER_REGISTER = DOMAIN + "user_register.php";
	//コンテンツのアップロード
	private string CONTENTS_UPLOAD = DOMAIN+"contents_upload.php";
	//ユーザ情報の取得
	private string GET_USER_INFOMATION = DOMAIN+"get_user_info.php";
	//ユーザ情報の更新
	private string UPDATE_USER_INFOMATION = DOMAIN + "update_user_info.php";

	private AnimationWebView webViewObject;

	//シングルトン
	private static WebManager _instance;
	public static WebManager instance {
		get{
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<WebManager> ();
			}
			return _instance;
		}
	}
	//WWWForm型のインスタンスを生成しアプリからの接続であることを証明するデータを付与したデータを返す
	private WWWForm getSecureForm(){
		WWWForm data = new WWWForm ();
		data.AddField ("app_auth",MyCommon.Common.AUTH_KEY);
		data.AddField ("token",SaveDataManager.loadToken());
		data.AddField ("user_name",SaveDataManager.loadUserName());
		return data;
	}
	public Coroutine contentsUpload(Action<string> positive_func,Action negative_func,RegisterContents item){
		WWWForm data = getSecureForm ();
		string itemJSON = UnityEngine.JsonUtility.ToJson (item);
		data.AddField ("contents",itemJSON);
		WWW www = new WWW (CONTENTS_UPLOAD,data);

		string[] sizes = { "B", "KB", "MB", "GB", "TB" };
		double len = item.getData ().Length;
		int order = 0;
		while (len >= 1024 && order < sizes.Length - 1) {
			order++;
			len = len/1024;
		}

		// Adjust the format string to your preferences. For example "{0:0.#}{1}" would
		// show a single decimal place, and no space.
		string result = String.Format("{0:0.##} {1}", len, sizes[order]);
		Debug.Log ("WebManager call contentsUpload");
		Debug.Log ("Upload file size => "+result);
		//Debug.Log ("JSON => "+itemJSON);

		return throwQueryToServer(www,positive_func,negative_func);
	}
	public Coroutine getUserInfomation(Action<string> positive_func,Action negative_func){
		WWWForm data = getSecureForm ();
		WWW www = new WWW (GET_USER_INFOMATION,data);
		Debug.Log ("WebManager call getUserInfomation");

		return throwQueryToServer(www,positive_func,negative_func);
	}
	public Coroutine updateUserInfomation(Action<string> positive_func,Action negative_func,Profile userInfo){
		WWWForm data = getSecureForm ();
		data.AddField ("user_info",UnityEngine.JsonUtility.ToJson(userInfo));
		WWW www = new WWW (UPDATE_USER_INFOMATION,data);
		Debug.Log ("WebManager call updateUserInfomation");


		return throwQueryToServer(www,positive_func,negative_func);
	}
	public void socialLogin(string snsType){
		string url = null;
		if (snsType == "facebook") {
			url = SOCIAL_LIGIN_FROM_FACEBOOK;
		} else if (snsType == "twitter") {
			url = SOCIAL_LOGIN_FROM_TWITTER;
		} else {
			return;
		}

		webViewObject = (new GameObject ("WebViewObject")).AddComponent<AnimationWebView> ();
		webViewObject.Init ((string msg)=>{

			msg = WWW.UnEscapeURL(msg);

			webViewObject.GetComponent<AnimationWebView> ().slideOut ();

			//URLに乗って情報が返されるのでDecodeする
			msg = MyLibrary.UriStringDecoder.decode (msg);
			//メッセージが短すぎると弾きます。せめてレスポンスタイプの指定をしてから返してね。
			if (msg.Length < "response_type".Length) {
				Debug.Log ("error");
				return;
			}

			Debug.Log (msg);

			Dictionary<string,object> resp = MiniJSON.Json.Deserialize (msg) as Dictionary<string,object>;

			if (resp == null)
				return;
			
			if ((string)resp ["response_type"] == "social_login") {
				GameObject.Find("SocialLoginController").SendMessage ("socialLogin",resp);
			}
		});
		webViewObject.LoadURL (url);
		webViewObject.slideIn ();
	}

	public Coroutine getSNSIcon(Action<Texture2D> success_func,Action failure_func,string url){
		WWW www = new WWW (url);

		Debug.Log ("WebManager call getSNSIcon");

		return throwQueryToServer (www,success_func,failure_func);
	}
	public Coroutine getResources(Action<Texture2D> success_func,Action failure_func,string filepath){
		WWWForm data = getSecureForm ();
		data.AddField ("filepath", filepath);
		WWW www = new WWW (GET_RESOURCES,data);

		Debug.Log ("WebManager call getResources");

		return throwQueryToServer (www,success_func,failure_func);
	}
	public Coroutine findUserName(Action<string> find_func,Action not_find_func,string user_name){
		WWWForm data = getSecureForm ();
		data.AddField ("user_name",user_name);
		WWW www = new WWW (FIND_USER_NAME,data);

		Debug.Log ("WebManager call findUserName");

		return throwQueryToServer (www,find_func,not_find_func);
	}
	public Coroutine autoLogin(Action<Dictionary<string,object>> positive_func,Action negative_func){
		WWWForm data = getSecureForm ();
		WWW www = new WWW (AUTO_LOGIN,data);

		Debug.Log ("WebManager call autoLogin");

		return throwQueryToServer (www,positive_func,negative_func);
	}
	public Coroutine downloadContents(Action<string> positive_func,Action negative_func,string mode){
		
		WWWForm data = getSecureForm ();
		data.AddField ("mode", mode);

		Debug.Log ("WebManager call downloadContents");


		WWW www = new WWW (FETCH_CONTENTS, data);

		return throwQueryToServer (www,positive_func,negative_func);
	}
	public Coroutine downloadContents(Action<string> positive_func,Action negative_func,string latitude,string longitude){

		WWWForm data = getSecureForm ();
		data.AddField ("mode", "location");
		data.AddField("latitude",latitude);
		data.AddField("longitude",longitude);

		Debug.Log ("WebManager call downloadContents");


		WWW www = new WWW (FETCH_CONTENTS, data);

		return throwQueryToServer (www,positive_func,negative_func);
	}
	public Coroutine login(Action<string> positive_func,Action negative_func,string user_name,string password){
		WWWForm data = getSecureForm ();
		data.AddField("user_name", user_name);
		data.AddField("password", password);
		WWW www = new WWW(LOGIN, data.data);

		Debug.Log ("WebManager call login");


		return throwQueryToServer (www,positive_func,negative_func);
	}
	public Coroutine socialLogin(Action<string> positive_func,Action negative_func,UserOfSNS user,string token){
		WWWForm data = getSecureForm ();
		data.AddField("user_name", user.name);
		data.AddField("social_id", user.id);
		data.AddField ("sns_type", user.socialType);
		data.AddField ("instant_token", token);
		WWW www = new WWW(LOGIN, data.data);

		Debug.Log ("WebManager call socialLogin");


		return throwQueryToServer (www,positive_func,negative_func);
	}
	public Coroutine socialRegister(Action<Dictionary<string,object>> positive_func,Action<string> negative_func,UserOfSNS user,string token){
		WWWForm data = getSecureForm ();
		string sns_type = "original";
		string icon_url = "original";
		if (user.GetType () == typeof(UserOfTwitter)) {
			sns_type = "twitter";
			UserOfTwitter t = (UserOfTwitter)user;
			icon_url = t.profile_image_url;
		} else if (user.GetType () == typeof(UserOfFacebook)) {
			sns_type = "facebook";
			icon_url = "https://graph.facebook.com/" + user.id + "/picture";
		}
		data.AddField("user_name", user.name);
		data.AddField("social_id", user.id);
		data.AddField ("sns_type", sns_type);
		data.AddField ("instant_token",token);
		data.AddField ("icon_url",icon_url);

		Debug.Log ("WebManager call socialRegister");


		WWW www = new WWW(USER_REGISTER, data.data);

		return throwQueryToServer (www,positive_func,negative_func);
	}
	public Coroutine contentsDump(Action<string> positive_func,Action negative_func,int contentsID,string title){

		WWWForm data = getSecureForm ();
		data.AddField("contents_id",contentsID);
		data.AddField ("title",title);
		WWW www = new WWW(CONTENTS_DUMP,data);
		Debug.Log ("WebManager call contentsDump");

		return throwQueryToServer (www,positive_func,negative_func);
	}
	public Coroutine contentsTaken(Action<string> positive_func,Action negative_func,int contentsID, string title){
		WWWForm data = getSecureForm ();
		data.AddField("contents_id",contentsID);
		data.AddField ("title", title);

		WWW www = new WWW(CONTENTS_TAKEN,data);

		Debug.Log ("WebManager call contentsTaken");
		return throwQueryToServer (www,positive_func,negative_func);
	}
	public Coroutine userRegister(Action<Dictionary<string,object>> positive_func,Action negative_func,string user_name,string password){
		WWWForm data = getSecureForm ();
		data.AddField ("user_name", user_name);
		data.AddField ("password",password);

		WWW www = new WWW (USER_REGISTER,data);
		Debug.Log ("WebManager call userRegister");


		return throwQueryToServer (www,positive_func,negative_func);
	}
	private Coroutine throwQueryToServer(WWW www,Action<string> positive_func,Action negative_func){
		IEnumerator e = ThrowQueryToServer (www, positive_func, negative_func);
		return StartCoroutine(e);
	}
	private Coroutine throwQueryToServer(WWW www,Action<string> positive_func,Action<string> negative_func){
		IEnumerator e = ThrowQueryToServer (www, positive_func, negative_func);
		return StartCoroutine(e);
		
	}
	private Coroutine throwQueryToServer(WWW www,Action<Dictionary<string,object>> positive_func,Action negative_func){
		IEnumerator e = ThrowQueryToServer (www, positive_func, negative_func);
		return StartCoroutine(e);
		
	}
	private Coroutine throwQueryToServer(WWW www,Action<Dictionary<string,object>> positive_func,Action<string> negative_func){
		IEnumerator e = ThrowQueryToServer (www, positive_func, negative_func);
		return StartCoroutine(e);
		
	}
	private Coroutine throwQueryToServer(WWW www,Action<Texture2D> positive_func,Action negative_func){
		IEnumerator e = ThrowQueryToServer (www, positive_func, negative_func);
		return StartCoroutine(e);
		
	}
	private IEnumerator ThrowQueryToServer(WWW www,Action<string> positive_func,Action negative_func){
		
		yield return www;
		Debug.Log (www.text);
		if (string.IsNullOrEmpty (www.error)) {
			
			string[] result = www.text.Split ('/');
			if (result [0] == MyCommon.Common.SUCCESS && result[0] != MyCommon.Common.FAILURE) {
				positive_func (www.text.Substring(MyCommon.Common.SUCCESS.Length+1));
			} else {
				negative_func ();
			}
		}
	}
	private IEnumerator ThrowQueryToServer(WWW www,Action<string> positive_func,Action<string> negative_func){
		yield return www;
		Debug.Log (www.text);
		if (string.IsNullOrEmpty (www.error)) {

			string[] result = www.text.Split ('/');
			if (result [0] == MyCommon.Common.SUCCESS && result[0] != MyCommon.Common.FAILURE) {
				positive_func (www.text.Substring(MyCommon.Common.SUCCESS.Length+1));
			} else {
				negative_func (www.text);
			}
		}
	}
	private IEnumerator ThrowQueryToServer(WWW www,Action<Dictionary<string,object>> positive_func,Action negative_func){
		yield return www;
		Debug.Log (www.text);
		if (string.IsNullOrEmpty (www.error)) {
			if (www.text != MyCommon.Common.FAILURE) {
				Dictionary<string,object> dic = MiniJSON.Json.Deserialize (www.text) as Dictionary<string,object>;
				Debug.Log (dic);
				if ((string)dic ["result"] == MyCommon.Common.SUCCESS) {
					positive_func (dic);
					yield break;
				}
			} 
			negative_func ();
		}

	}
	private IEnumerator ThrowQueryToServer(WWW www,Action<Dictionary<string,object>> positive_func,Action<string> negative_func){
		yield return www;
		Debug.Log (www.text);
		if (string.IsNullOrEmpty (www.error)) {
			if (www.text != MyCommon.Common.FAILURE) {
				Dictionary<string,object> dic = MiniJSON.Json.Deserialize (www.text) as Dictionary<string,object>;
				Debug.Log (dic);
				if ((string)dic ["result"] == MyCommon.Common.SUCCESS) {
					positive_func (dic);
					yield break;
				}
			} 
			negative_func (www.text);
		}
	}

	private IEnumerator ThrowQueryToServer(WWW www,Action<Texture2D> positive_func,Action negative_func){
		yield return www;
		if (string.IsNullOrEmpty (www.error)&&www.text != MyCommon.Common.FAILURE) {
			positive_func (www.texture);
		} else {
			negative_func ();
		}
	}
}

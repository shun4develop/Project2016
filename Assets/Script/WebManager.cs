using UnityEngine;
using System.Collections;
using System;
using MyManagers;
using UnityEngine.UI;
using MyClass;

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
	public void contentsUpload(Action<string> positive_func,Action negative_func,RegisterContents item){
		WWWForm data = getSecureForm ();
		data.AddField ("contents",UnityEngine.JsonUtility.ToJson(item));
		WWW www = new WWW (CONTENTS_UPLOAD,data);

		throwQueryToServer(www,positive_func,negative_func);
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
		webViewObject.Init ();
		webViewObject.LoadURL (url);
		webViewObject.slideIn ();
	}

	public void getSNSIcon(Action<Texture2D> success_func,Action failure_func,string url){
		WWW www = new WWW (url);
		throwQueryToServer (www,success_func,failure_func);
	}
	public void getResources(Action<Texture2D> success_func,Action failure_func,string filepath){
		WWWForm data = getSecureForm ();
		data.AddField ("filepath", filepath);
		WWW www = new WWW (GET_RESOURCES,data);
		throwQueryToServer (www,success_func,failure_func);
	}
	public void findUserName(Action<string> find_func,Action not_find_func,string user_name){
		WWWForm data = getSecureForm ();
		data.AddField ("user_name",user_name);
		WWW www = new WWW (FIND_USER_NAME,data);
		throwQueryToServer (www,find_func,not_find_func);
	}
	public void autoLogin(Action<string> positive_func,Action negative_func){
		WWWForm data = getSecureForm ();
		WWW www = new WWW (AUTO_LOGIN,data);
		throwQueryToServer (www,positive_func,negative_func);
	}
	public void downloadContents(Action<string> positive_func,Action negative_func,string mode){
		
		WWWForm data = getSecureForm ();
		data.AddField ("mode", mode);

		WWW www = new WWW (FETCH_CONTENTS, data);

		throwQueryToServer (www,positive_func,negative_func);
	}public void downloadContents(Action<string> positive_func,Action negative_func,string latitude,string longitude){

		WWWForm data = getSecureForm ();
		data.AddField ("mode", "location");
		data.AddField("latitude",latitude);
		data.AddField("longitude",longitude);

		WWW www = new WWW (FETCH_CONTENTS, data);

		throwQueryToServer (www,positive_func,negative_func);
	}
	public void login(Action<string> positive_func,Action negative_func,string user_name,string password){
		WWWForm data = getSecureForm ();
		data.AddField("user_name", user_name);
		data.AddField("password", password);
		WWW www = new WWW(LOGIN, data.data);

		throwQueryToServer (www,positive_func,negative_func);
	}
	public void socialLogin(Action<string> positive_func,Action negative_func,UserOfSNS user,string token){
		WWWForm data = getSecureForm ();
		data.AddField("user_name", user.name);
		data.AddField("social_id", user.id);
		data.AddField ("sns_type", user.socialType);
		data.AddField ("instant_token", token);
		WWW www = new WWW(LOGIN, data.data);

		throwQueryToServer (www,positive_func,negative_func);
	}
	public void socialRegister(Action<string> positive_func,Action negative_func,UserOfSNS user,string token){
		WWWForm data = getSecureForm ();
		string sns_type = "original";
		if(user.GetType() == typeof(UserOfTwitter))
			sns_type = "twitter";
		else if(user.GetType() == typeof(UserOfFacebook))
			sns_type = "facebook";
		data.AddField("user_name", user.name);
		data.AddField("social_id", user.id);
		data.AddField ("sns_type", sns_type);
		data.AddField ("instant_token",token);
		WWW www = new WWW(USER_REGISTER, data.data);

		throwQueryToServer (www,positive_func,negative_func);
	}
	public void contentsDump(Action<string> positive_func,Action negative_func,int contentsID,string title){

		WWWForm data = getSecureForm ();
		data.AddField("contents_id",contentsID);
		data.AddField ("title",title);
		WWW www = new WWW(CONTENTS_DUMP,data);

		throwQueryToServer (www,positive_func,negative_func);
	}
	public void contentsTaken(Action<string> positive_func,Action negative_func,int contentsID, string title){
		WWWForm data = getSecureForm ();
		data.AddField("contents_id",contentsID);
		data.AddField ("title", title);

		WWW www = new WWW(CONTENTS_TAKEN,data);

		throwQueryToServer (www,positive_func,negative_func);
	}
	public void userRegister(Action<string> positive_func,Action negative_func,string user_name,string password){
		WWWForm data = getSecureForm ();
		data.AddField ("user_name", user_name);
		data.AddField ("password",password);

		WWW www = new WWW (USER_REGISTER,data);

		throwQueryToServer (www,positive_func,negative_func);
	}
	private void throwQueryToServer(WWW www,Action<string> positive_func,Action negative_func){
		StartCoroutine (ThrowQueryToServer(www,positive_func,negative_func));
	}
	private void throwQueryToServer(WWW www,Action<Texture2D> positive_func,Action negative_func){
		StartCoroutine (ThrowQueryToServer(www,positive_func,negative_func));
	}
	private IEnumerator ThrowQueryToServer(WWW www,Action<string> positive_func,Action negative_func){
		yield return www;
		Debug.Log (www.text);
//		Debug.Log (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		if (string.IsNullOrEmpty (www.error)) {
			string[] result = www.text.Split ('/');
			if (result [0] == MyCommon.Common.SUCCESS && result[0] != MyCommon.Common.FAILURE) {
				positive_func (www.text.Substring(MyCommon.Common.SUCCESS.Length+1));
			} else {
				negative_func ();
			}
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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
namespace MapScene{
	public class SelectImage : MonoBehaviour {

		public GameObject webManager;
		private string path;
		private string base64data;
		private Sprite sp;
		private AndroidJavaClass unityPlayer;
		private AndroidJavaObject activity;
		private GameObject mapobj;
		public Text t;
		public GameObject detailPanel;

		private void Start(){

			unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
			mapobj = GameObject.Find( "Map" );

		}

		public void ShowDialog(){
			activity.CallStatic("ShowAlertDialog", activity);
		}

		//pluginからファイルパスが返ってくる
		public void onCallBack (string msg)
		{
			Debug.Log ("Call From Native. (" + msg + ")");
			//imagepath.text = msg;
			path = msg;
			//m.mainTexture = ReadTexture(msg,50, 50);
			ReadTexture(msg,50, 50);
		}

		//base64data texture2D Sprite作成
		public void ReadTexture(string strPath, int intWidth, int intHeight)
		{
			// バイト配列でファイルを読み込み、Texture2Dとしてセットする.
			byte[] byteReadBinary = File.ReadAllBytes(strPath);
			base64data = System.Convert.ToBase64String (byteReadBinary); //base64に変換
			Texture2D txtNewImage = new Texture2D(intWidth, intHeight);
			txtNewImage.LoadImage(byteReadBinary);
			sp = Sprite.Create(txtNewImage, new Rect(0, 0, txtNewImage.width, txtNewImage.height),Vector2.zero);
			//		img.sprite = sp;

			double lat=0,lon=0;

			//MapのOnlineMapsLocationServiceを取ってくる
			OnlineMapsLocationService location = mapobj.GetComponent<OnlineMapsLocationService>();
			lat = location.position.y;
			lon = location.position.x;

			//詳細画面base64dataとLocationとSpriteImageを渡す
			InputDetailInfoCanvas detail = detailPanel.GetComponent<InputDetailInfoCanvas> ();
			detail.setBase64data (base64data);
			detail.setLocation (lat, lon);
			detail.setSpriteImage (sp);

			//animationを用いて詳細画面を表示する
			AnimationUI ui = detailPanel.GetComponent<AnimationUI> ();
			ui.fadeIn ();
		}
	}

}
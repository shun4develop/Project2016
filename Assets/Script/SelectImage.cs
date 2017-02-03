using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
namespace MapScene{
	public class SelectImage : MonoBehaviour {

		public GameObject webManager;
		public Text t;
		public GameObject detailPanel;
		public GameObject mapobj;

		private MapControl mapcontrol;
		private string base64data;
		private Sprite sp;
		private AndroidJavaClass unityPlayer;
		private AndroidJavaObject activity;

		private void Start(){
			
			mapcontrol = mapobj.GetComponent<MapControl> ();

		}

		public void ShowDialog(){
			if (Application.platform == RuntimePlatform.Android) {
				unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
				activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
				activity.CallStatic("ShowAlertDialog", activity);
			} else if(Application.platform == RuntimePlatform.IPhonePlayer) {
				IOSPlugin.choice ();
			}
		}

		//pluginからファイルパスが返ってくる
		public void onCallBack (string msg)
		{
			Debug.Log ("Call From Native. (" + msg + ")");
			ReadTexture(msg,100, 100);

			//animationを用いて詳細画面を表示する
			AnimationUI ui = detailPanel.GetComponent<AnimationUI> ();
			ui.fadeIn ();
		}

		//base64data texture2D Sprite作成
		public void ReadTexture(string strPath, int intWidth, int intHeight)
		{
			// バイト配列でファイルを読み込み、Texture2Dとしてセットする.
			byte[] byteReadBinary = File.ReadAllBytes(strPath);
			base64data = System.Convert.ToBase64String (byteReadBinary); //base64に変換
			Texture2D txtNewImage = new Texture2D (intWidth, intHeight);

			//texture2dとspritを作る
			if (Application.platform == RuntimePlatform.Android) {
				txtNewImage.LoadImage(byteReadBinary);
				sp = Sprite.Create(txtNewImage, new Rect(0, 0, txtNewImage.width, txtNewImage.height),new Vector2 (0.5f, 0.5f));
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
//				Texture2D txtNewImage = new Texture2D (intWidth, intHeight, TextureFormat.PVRTC_RGBA4, false);
				txtNewImage.LoadImage(byteReadBinary);
				sp = Sprite.Create(txtNewImage, new Rect(0, 0, txtNewImage.width, txtNewImage.height),new Vector2 (0.5f, 0.5f));
			}


			//byte[] binaryData = txtNewImage.EncodeToPNG ();

			//txtNewImage.LoadImage(byteReadBinary);
			//sp = Sprite.Create(txtNewImage, new Rect(0, 0, txtNewImage.width, txtNewImage.height),new Vector2 (0.5f, 0.5f));
			//t.text += "width" + txtNewImage.width.ToString () + " + " + "height" + txtNewImage.height.ToString ();
			//double lat=0,lon=0;

			//MapのOnlineMapsLocationServiceを取ってくる
//			OnlineMapsLocationService location = mapobj.GetComponent<OnlineMapsLocationService>();
//			lat = location.position.y;
//			lon = location.position.x;

			//詳細画面 dataとLocationとSpriteImageを渡す
			InputDetailInfoCanvas detail = detailPanel.GetComponent<InputDetailInfoCanvas> ();

			detail.cameraCullingMaskChange (0);
			detail.setBinaryData (byteReadBinary);

//			detail.setLocation (lat, lon);
			detail.setSpriteImage (sp);
			detail.inputfieldclear ();
		}
	}

}
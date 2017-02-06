using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
namespace MapScene{
	public class SelectImage : MonoBehaviour {

		public GameObject webManager;
		public GameObject detailPanel;
		public GameObject mapobj;

		private Sprite sp;
		private AndroidJavaClass unityPlayer;
		private AndroidJavaObject activity;

		private void Start(){
			

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



			//詳細画面 dataとLocationとSpriteImageを渡す
			InputDetailInfoCanvas detail = detailPanel.GetComponent<InputDetailInfoCanvas> ();

			detail.setBinaryData (byteReadBinary);

			detail.setSpriteImage (sp);
			detail.inputfieldclear ();
		}
	}

}
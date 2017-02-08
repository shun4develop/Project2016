using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using MyManagers;
using MyClass;

public class InputDetailInfoCanvas : MonoBehaviour{

	private RegisterContents content;
	private byte[] binaryData;
	private double lat;
	private double lon;
	private Sprite sp;

	public InputField title;
	public InputField desc;
	public Toggle toggle;
	public ContentsOfImage img;
	public Text logText;
	public ImageFullPanel fullImage;
	public UploadLogCanvas uploadLog;
	public MapControl map;
		
	public void setSpriteImage(Sprite sp){
		this.sp = sp;

		img.setTexture (this.sp);

		fullImage.setSprite (this.sp);
	}

	public void setBinaryData(byte[] binary){
		this.binaryData = binary;
	}

	public void inputfieldclear(){
		title.text = "";
		desc.text = "";
	}


	//RegisterContents型のデータをアップロードする
	public void uploadContent(){
		
		if (title.text == "") {
			logText.text = "Titleが入力されていません";
			StartCoroutine (wordDelete(2));
			return;
		}

		double lat = LocationManager.location.latitude;
		double lon = LocationManager.location.longitude;

		content = new RegisterContents (desc.text, title.text, lat, lon ,SaveDataManager.loadUserName() , toggle.isOn, "images");

		Action<string> positive_func = (string text) => {
			LoadingManager.stop();

			this.gameObject.GetComponent<AnimationUI> ().fadeOut();
			map.updateMap();
			uploadLog.uploadComplete();

			clear();
		};

		Action negative_func = () => {
			LoadingManager.stop();
			uploadLog.uploadFailure();
		};

		WebManager.instance.contentsUpload (positive_func, negative_func, content,binaryData);

		LoadingManager.run ();

	}
	public void clear(){
		title.text = "";
		desc.text = "";
		toggle.isOn = true;
		img.clearImage ();
		fullImage.clearImage ();
	}
	private IEnumerator wordDelete(int sec){
		yield return new WaitForSeconds (sec);
		logText.text = "";
	}
}

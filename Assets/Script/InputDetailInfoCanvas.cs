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
	public ImageFullPanel fullImage;
	public GameObject uploadCheck;
	public Text debugText;
	public Camera camera;
		
	public void setSpriteImage(Sprite sp){
		this.sp = sp;

		img.setTexture (this.sp);

		fullImage.setSprite (this.sp);
	}

	public void setBinaryData(byte[] binary){
		this.binaryData = binary;
	}

	public void cameraCullingMaskChange(int depth){
		camera.cullingMask = depth;
	}

	public void inputfieldclear(){
		title.text = "";
		desc.text = "";
	}

//	public void setLocation(double lat , double lon){
//		this.lat = lat;
//		this.lon = lon;
//	}

	//RegisterContents型のデータをアップロードする
	public void uploadContent(){
		
		if (title.text == "") {
			title.text = "untitle";
		}

		if (desc.text == "") {
			desc.text = "no comment";
		}

		double lat = LocationManager.location.latitude;
		double lon = LocationManager.location.longitude;

		content = new RegisterContents (desc.text, title.text, lat, lon ,SaveDataManager.loadUserName() , toggle.isOn, "images");

		Action<string> positive_func = (string text) => {
			debugText.text += text;
			AnimationUI u = uploadCheck.GetComponent<AnimationUI>();
			u.fadeIn();
		};

		Action negative_func = () => {
			Debug.Log("miss");
		};

		WebManager.instance.contentsUpload (positive_func, negative_func, content,binaryData);
		AnimationUI ui = this.gameObject.GetComponent<AnimationUI> ();
		ui.fadeOut ();
<<<<<<< HEAD
		clear ();
	}
	public void clear(){
		title.text = "";
		desc.text = "";
		toggle.isOn = true;
		img.clearImage ();
		fullImage.clearImage ();
=======
		cameraCullingMaskChange (1);
>>>>>>> master
	}
}

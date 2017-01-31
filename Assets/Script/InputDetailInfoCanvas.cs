﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using MyManagers;
using MyClass;

public class InputDetailInfoCanvas : MonoBehaviour{

	private RegisterContents content;

	private string binaryData;
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
		
	public void setSpriteImage(Sprite sp){
		this.sp = sp;

		img.setTexture (this.sp);

		fullImage.setSprite (this.sp);
	}

	public void setBinaryData(string base64data){
		this.binaryData = base64data;
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

		content = new RegisterContents (binaryData, desc.text, title.text, lat, lon ,SaveDataManager.loadUserName() , toggle.isOn, "images");

		Action<string> positive_func = (string text) => {
			debugText.text += text;
			AnimationUI u = uploadCheck.GetComponent<AnimationUI>();
			u.fadeIn();
		};

		Action negative_func = () => {
			Debug.Log("miss");
		};

		WebManager.instance.contentsUpload (positive_func, negative_func, content);
		AnimationUI ui = this.gameObject.GetComponent<AnimationUI> ();
		ui.fadeOut ();
		clear ();
	}
	public void clear(){
		title.text = "";
		desc.text = "";
		toggle.isOn = true;
		img.clearImage ();
		fullImage.clearImage ();
	}
}

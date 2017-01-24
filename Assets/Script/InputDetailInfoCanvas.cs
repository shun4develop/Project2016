using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using MyManagers;
using MyClass;
/// <summary>
/// </summary>


public class InputDetailInfoCanvas : MonoBehaviour{

	private RegisterContents content;
	private Item item;

	private string base64data;
	private double lat;
	private double lon;
	private bool permitSave = true;
	private Sprite sp;

	public InputField title;
	public InputField desc;
	public Toggle toggle;
	public Image img;
	public GameObject fullImagePanel;
	public GameObject uploadCheck;
	public Text debugText;

	void Start(){
		toggle.onValueChanged.AddListener(OnValueChanged);
	}
		
	public void OnValueChanged(bool value){
		permitSave = value;
	}
		
	public void setSpriteImage(Sprite sp){
		this.sp = sp;
		img.sprite = sp;

		ImageFullPanel fullImage = fullImagePanel.GetComponent<ImageFullPanel> ();
		fullImage.setSprite (sp);
	}

	public void setBase64data(string base64data){
		this.base64data = base64data;
	}

	public void setLocation(double lat , double lon){
		this.lat = lat;
		this.lon = lon;
	}

	//RegisterContents型のデータをアップロードする
	public void uploadContent(){
		
		if (title.text == "") {
			title.text = "no title";
		}

		if (desc.text == "") {
			desc.text = "no comment";
		}

		content = new RegisterContents (base64data, desc.text, title.text, lat, lon ,SaveDataManager.loadUserName() , permitSave, "images");

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
	}
}

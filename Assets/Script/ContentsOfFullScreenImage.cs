using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
[RequireComponent(typeof(Image))]
public class ContentsOfFullScreenImage : ContentsViewerBase {

	private Image image;

	void Start(){
		image = GetComponent<Image> ();
	}
		
	public override void show(){
		
	}

	//imageのオブジェクトに貼り付ける
	public override void setTexture(Texture2D tex){
		try{
			Sprite s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
			setTexture (s);
		}catch{
			Debug.Log ("すでにオブジェクトが破棄されています。");
		}
	}
	public  void setTexture(Sprite s){
		try{
			image.sprite = s;
			showCompleted = true;
		}catch{
			Debug.Log ("すでにオブジェクトが破棄されています。");
		}
	}
	public void clearImage(){
		image.sprite = null;
		showCompleted = false;
	}
}

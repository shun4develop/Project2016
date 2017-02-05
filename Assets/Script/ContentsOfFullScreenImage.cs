using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class ContentsOfFullScreenImage : ContentsViewerBase {

	private Image image;

	void Start(){
		image = GetComponent<Image> ();
	}

	private Coroutine runningCoroutine;

	public override void show(){
		
	}

	//imageのオブジェクトに貼り付ける
	public override void setTexture(Texture2D tex){
		try{
			Sprite s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
			setTexture (s);
			ItemData.instance.addSprite (Item.getId(), s);
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
		try{
			StopCoroutine (runningCoroutine);
		}catch{
			Debug.Log ("すでにCoroutineが終了しています。");
		}finally{
			image.sprite = null;
			showCompleted = false;
		}

	}
}

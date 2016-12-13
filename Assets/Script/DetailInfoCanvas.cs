using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using MyClass;
using MyLibrary;
/// <summary>
/// 
///  詳細画面用の Canvas に貼り付ける
/// 
///  Canvas のなかに情報を入れていく
/// 
/// </summary>


public class DetailInfoCanvas : ContentInfoCanvasBase{

	public Text title;
	public Text date;
	public Text comment;

	GameObject child;

	public GameObject btn;

	public override void init(Item item){
		base.init (item);
		this.date.text = item.getDate ();
		this.title.text = item.getTitle ();
		this.comment.text = item.getDesc ();

		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Camera"
		    && !ItemData.instance.checkOverlapItemById (item.getId ())) {
			Debug.Log ("すでに持ってる" + item);
			btn.GetComponent<Button> ().interactable = false;
		} else {
			btn.GetComponent<Button>().interactable = true;
		}

		base.show ();
		//base.fadeIn ();
	}
}

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
///  詳細画面用の Panel にアタッチ
/// 
///  Panel に情報を入れる
/// 
/// </summary>


public class DetailInfoCanvas : MonoBehaviour{

	public Item item{ set; get;}

	public Text title;
	public Text date;
	public Text comment;
	public GameObject image;

	public GameObject btn;

	private ContentsViewerBase iv;

	GameObject child;

	public void init(Item item){
		this.iv = image.GetComponent<ContentsOfImage> ();
		this.iv.Item = item;

		this.item = item;
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

		if (this.item == null) {
			return;
		}
		iv.show ();
	}
}

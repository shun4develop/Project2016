﻿using UnityEngine;
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
	public Text owner;
	public Image image;
	public Button btn;

	private ContentsViewerBase imageViewer;

	void Start(){
		this.imageViewer = image.GetComponent<ContentsViewerBase> ();
	}

	public void init(Item item){
		this.imageViewer.Item = item;
		this.item = item;
		this.date.text = item.getDate ().Substring(0,10);
		this.title.text = item.getTitle ();
		this.comment.text = item.getDesc ();
		if (string.IsNullOrEmpty (this.comment.text)) {
			this.comment.text = "<color=#c0c0c0ff>コメントはありません</color>";
		}
		this.owner.text = "<color=blue>@"+item.getOwner ()+"</color>";

		if ((UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Camera"
			&& (ItemData.instance.checkOverlapItemById (item.getId ()) || !item.getPermitSave()))) {
			btn.interactable = false;
			btn.GetComponent<CanvasGroup> ().alpha = 0;
		} else {
			btn.interactable = true;
			btn.GetComponent<CanvasGroup> ().alpha = 1;
		}

		if (this.item == null) {
			return;
		}

		imageViewer.show ();

	}
}

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

	public override void init(Item item){
		base.init (item);
		this.date.text = item.getDate ();
		this.title.text = item.getTitle ();
		this.comment.text = item.getDesc ();

//		if (item.getPermitSave () == true) {
//			child = transform.FindChild ("SaveButton").gameObject;
//		} else {
//			child = transform.FindChild ("DeleteButton").gameObject;
//		}
//
//

		//child.SetActive (true);
		//addListener (child);

		base.show ();
		//base.fadeIn ();
	}

	public override void init(int id){
		base.init (id);
		this.date.text = this.item.getDate();
		this.title.text = this.item.getTitle();
		this.comment.text = this.item.getDesc();
		base.show ();
//		base.fadeIn ();
	}

//	public void addListener(GameObject obj){
//		obj.GetComponent<Button>().onClick.AddListener (() => {
//			enter();
//			Debug.Log(this.item);;
//		});
//	}
//
//
//	public void enter(){
//		if (this.item.getPermitSave () == false) {
//			contentDelete (this.item.getId (), this.item.getTitle());
//		} else {
//			Debug.Log ("true / " + this.item.getId());
//			contentSave (this.item.getId(), this.item.getTitle());
//		}
//	}
}

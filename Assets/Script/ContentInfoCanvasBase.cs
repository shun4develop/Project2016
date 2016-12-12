﻿using UnityEngine;
using System.Collections;
using MyClass;

public abstract class ContentInfoCanvasBase : MonoBehaviour {
	public Item item{ set; get;}
	public GameObject image;
	private ContentsViewerBase iv;

	public virtual void init(Item item){
		this.iv = image.GetComponent<ContentsOfImage> ();
		this.iv.Item = item;
		this.item = item;
	}
//	public virtual void init(int id){
//		this.iv = image.GetComponent<ContentsOfImage> ();
//		this.iv.Item = item;
//		this.item = ItemData.instance.getItemById(id);
//	}
	public void show(){
		if (this.item == null) {
			return;
		}
		iv.show ();
	}
//	public void fadeIn(){
//		gameObject.SendMessage ("FadeIn");
//	}
}

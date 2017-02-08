﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(RectTransform))]
public class ScrollControllWithElementSizeOfChildren : MonoBehaviour{

	public RectTransform[] contents;

	private RectTransform rect;
	private bool move = false;


	void Start(){
		rect = GetComponent<RectTransform> ();
	}

	public void fitWithAnimation ()
	{
		if (GetComponent<iTween> () == null && !move) {
			if (rect.localPosition.y < 0) {
				move = true;
				Hashtable hash = new Hashtable ();
				hash.Add ("from", rect.anchoredPosition.y);
				hash.Add ("to", 0);
				hash.Add ("time", 0.2f);
				hash.Add ("easeType", iTween.EaseType.linear);
				hash.Add ("onupdate", "scroll");
				hash.Add ("oncompletetarget", gameObject);
				hash.Add ("oncomplete", "fitComplete");

				iTween.ValueTo (gameObject, hash);
			} else if (rect.localPosition.y > getContentsSizeDelta ()) {
				move = true;
				Hashtable hash = new Hashtable ();
				hash.Add ("from", rect.localPosition.y);
				hash.Add ("to", getContentsSizeDelta ());
				hash.Add ("time", 0.2f);
				hash.Add ("easeType", iTween.EaseType.linear);
				hash.Add ("onupdate", "scroll");
				hash.Add ("oncompletetarget", gameObject);
				hash.Add ("oncomplete", "fitComplete");

				iTween.ValueTo (gameObject, hash);
			} else {
				move = false;
			}
		}
	}
	public void fit(){
		if (rect.localPosition.y < -1) {
			scroll (0);
		} else if (rect.localPosition.y > getContentsSizeDelta ()+1) {
			scroll (getContentsSizeDelta());
		} 
	}
	private float getContentsSizeDelta(){
		float size = 0;
		foreach(RectTransform rt in contents){
			size += rt.sizeDelta.y;
		}
		return size;
	}
	private void scroll(float val){
		rect.localPosition = new Vector2(0,val);
	}
	private void fitComplete(){
		move = false;
	}
}
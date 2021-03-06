﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Indicator : MonoBehaviour {

	private ContentsViewerBase contents;
	private Image image;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
	}
	// Update is called once per frame
	void Update () {
		contents = transform.parent.GetComponent<ContentsViewerBase> ();
		if ( contents == null || !contents.showCompleted) {
			image.enabled = true;
			iTween.RotateTo (gameObject, iTween.Hash ("z", -720, "easetype", "linear", "loopType", "loop", "delay", 0));
		} else {
			image.enabled = false;
			GetComponent<RectTransform> ().rotation = new Quaternion(0,0,0,1);
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Indicator : MonoBehaviour {

	private ContentsViewerBase contents;
	private Image image;

	// Use this for initialization
	void Start () {
		GameObject parent = transform.parent.gameObject;
		contents = parent.GetComponent<ContentsViewerBase> ();

		image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!contents.showCompleted) {
			image.enabled = true;
			iTween.RotateTo (gameObject, iTween.Hash ("z", -720, "easetype", "linear", "loopType", "loop", "delay", 0));
		} else {
			image.enabled = false;
			GetComponent<RectTransform> ().rotation = new Quaternion(0,0,0,1);
			//iTween.Stop ("Rotate");
		}
	}
}

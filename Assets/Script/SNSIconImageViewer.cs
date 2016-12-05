using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
public class SNSIconImageViewer : MonoBehaviour {
	public void show(string filepath){
		Action<Texture2D> success = (Texture2D tex) => {
			GetComponent<Image>().sprite =  Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		};
		Action failure = () => {
			Destroy(this.gameObject,3f);
		};
		WebManager.instance.getSNSIcon (success,failure,filepath);
	}
}

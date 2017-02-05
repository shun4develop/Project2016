using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using MyManagers;
public class IconImageViewer : MonoBehaviour {
	public void show(string filepath){
		
		Action<Texture2D> success = (Texture2D tex) => {
			GetComponent<Image> ().sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		};
		Action failure = () => {
			GetComponent<Image> ().sprite = Resources.Load<Sprite> ("image/DefaultIcon");
		};
		WebManager.instance.getSNSIcon (success, failure, filepath);
	}
}

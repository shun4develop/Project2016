using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//public class PassImageToFullScreenPanel: MonoBehaviour
public class ImageFullPanel: MonoBehaviour{
	
	public Image image;

	public void setSprite(Sprite sp){
		image.sprite = sp;
	}
}

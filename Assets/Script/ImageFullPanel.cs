using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//public class PassImageToFullScreenPanel: MonoBehaviour
public class ImageFullPanel: MonoBehaviour{
	
	public Image image;

	public void setSprite(Sprite s){
		image.sprite = s;
	}

	public void clearImage(){
		image.sprite = null;
	}
}

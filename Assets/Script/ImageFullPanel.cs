using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageFullPanel: MonoBehaviour{
	
	public ContentsOfFullScreenImage image;

	public void setSprite(Sprite s){
		image.setTexture (s);
	}

	public void clearImage(){
		image.clearImage ();
	}
}

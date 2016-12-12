using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class ContentsOfImage : ContentsViewerBase {

	public Image fullImage;


	public override void show(){
//		Texture2D downloadedTex = ItemData.instance.getContentsTexture2DById (Item.getId());
		Sprite s = ItemData.instance.getContentsSpriteById (Item.getId());

		if (s != null) {
			setSprite (s);
		} else {
			Action<Texture2D> success_func = (Texture2D tex) => {
				setTexture (tex);
				//ItemData.instance.addContents(Item.getId(),tex);
				showCompleted = true;
			};
			Action failure_func = () => {
				Debug.Log ("ContentsOfImage : failure_func");
			};
			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Album") {
				WebManager.instance.getResources (success_func, failure_func, ItemData.instance.getBagItemById (Item.getId ()).getFilepath ());
			} else {
				WebManager.instance.getResources (success_func, failure_func, ItemData.instance.getLocationItemById (Item.getId ()).getFilepath ());
			}
		}

	}


	//imageのオブジェクトに貼り付ける
	public override void setTexture(Texture2D tex){
		Sprite s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		ItemData.instance.addSprite (Item.getId(), s);
		Image image = this.gameObject.GetComponent<Image> ();
		image.sprite = s;
		fullImage.GetComponent<Image> ().sprite = s;
//
		showCompleted = true;
	}

	public  void setSprite(Sprite s){
//		Sprite s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		Image image = this.gameObject.GetComponent<Image> ();
		image.sprite = s;
		fullImage.GetComponent<Image> ().sprite = s;
		//
		showCompleted = true;
	}

	public void clearIame(){
		this.gameObject.GetComponent<Image> ().sprite  = null;
		fullImage.GetComponent<Image> ().sprite = null;
		showCompleted = false;
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class ContentsOfImage : ContentsViewerBase {

	private Image image;
	private ImageFullPanel ifp;

	public bool loading = false;


	void Start(){
		image = this.gameObject.GetComponent<Image> ();
		ifp = this.GetComponent<ImageFullPanel> ();
		clearImage ();
	}


	public override void show(){

		Sprite s = ItemData.instance.getContentsSpriteById (Item.getId());

		if (s != null) {
			setSprite (s);
		} else {
			Action<Texture2D> success_func = (Texture2D tex) => {
				setTexture (tex);
				ItemData.instance.addContents(Item.getId(),tex);

				loading = false;
			};
			Action failure_func = () => {
				Debug.Log ("ContentsOfImage : failure_func");
			};
			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Album") {
				// Scene が Album の場合は BagItem を参照する
				WebManager.instance.getResources (success_func, failure_func, ItemData.instance.getBagItemById (Item.getId ()).getFilepath ());
			} else {
				// それ以外の Scene（Camera）では LovationItem をもらう
				WebManager.instance.getResources (success_func, failure_func, ItemData.instance.getLocationItemById (Item.getId ()).getFilepath ());
			}
			loading = true;
		}

	}


	//imageのオブジェクトに貼り付ける
	public override void setTexture(Texture2D tex){
		Sprite s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		image.sprite = s;
		ifp.setSprite (s);

		ItemData.instance.addSprite (Item.getId(), s);
		showCompleted = true;
	}

	public  void setSprite(Sprite s){
		image.sprite = s;
		ifp.setSprite (s);
		showCompleted = true;
	}

	public void clearImage(){
		image.sprite = null;
		ifp.clearImage();
		showCompleted = false;
	}
}

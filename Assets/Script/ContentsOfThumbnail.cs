using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using MyClass;


/// <summary>
/// 画像をもらって貼り付ける
/// </summary>

public class ContentsOfThumbnail : ContentsViewerBase  {

	public override void show ()
	{
		Texture2D downloadedTex = ItemData.instance.getThumbnailTexture2DById (Item.getId());
		if (downloadedTex != null) {
			setTexture (downloadedTex);
		} else {
			Action<Texture2D> success_func = (Texture2D tex) => {
				setTexture (tex);
				ItemData.instance.addThumbnail(Item.getId(),tex);
				showCompleted = true;
			};
			Action failure_func = () => {
				Debug.Log ("ContentsOfThumbnail : failure_func");
			};
			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Album") {
				// Scene が Album の場合は BagItem を参照する
				WebManager.instance.getResources (success_func, failure_func, ItemData.instance.getBagItemById (Item.getId ()).getThumbnail ());
			} else {
<<<<<<< Updated upstream
				// それ以外の Scene（Camera）では LovationItem をもらう
=======
				// それ以外の Scene（Camera）では LocationItem をもらう
>>>>>>> Stashed changes
				WebManager.instance.getResources (success_func, failure_func, ItemData.instance.getLocationItemById (Item.getId ()).getThumbnail ());
			}
		}

	}		
	//imageのオブジェクトに貼り付ける
	public override void setTexture(Texture2D tex){
		Sprite s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		Image image = this.gameObject.GetComponent<Image> ();
		image.sprite = s;
		showCompleted = true;
	}
}

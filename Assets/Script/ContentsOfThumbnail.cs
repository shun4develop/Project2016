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
			WebManager.instance.getResources (success_func, failure_func, ItemData.instance.getItemById (Item.getId()).getThumbnail ());
		}

	}
	public override void show(string filepath){
		//StartCoroutine (getImage (filepath));
//		Debug.Log (Item);
	}

	public override void show(int id){
		// Texture2D tex = CLASS.DownloadedContents(filepath)

		Texture2D tex = ItemData.instance.getThumbnailTexture2DById (id);
		if (tex != null) {
			setTexture (tex);
		} else {
			Action<Texture2D> success_func = (Texture2D tex2) => {
				setTexture (tex2);
			};
			Action failure_func = () => {
				Debug.Log ("ContentsOfThumbnail : failure_func");
			};
			WebManager.instance.getResources (success_func, failure_func, ItemData.instance.getItemById (id).getThumbnail ());
		}


		//StartCoroutine (getImage (filepath));
	}
		
	//imageのオブジェクトに貼り付ける
	public override void setTexture(Texture2D tex){
		Sprite s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		Image image = this.gameObject.GetComponent<Image> ();
		image.sprite = s;

		showCompleted = true;
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class ContentsOfImage : ContentsViewerBase {

	public Image fullImage;

	public override void show(){
		Texture2D downloadedTex = ItemData.instance.getContentsTexture2DById (Item.getId());
		if (downloadedTex != null) {
			setTexture (downloadedTex);
		} else {
			Action<Texture2D> success_func = (Texture2D tex) => {
				setTexture (tex);
				ItemData.instance.addContents(Item.getId(),tex);
				showCompleted = true;
			};
			Action failure_func = () => {
				Debug.Log ("ContentsOfImage : failure_func");
			};
			WebManager.instance.getResources (success_func, failure_func, ItemData.instance.getItemById (Item.getId()).getThumbnail ());
		}
	}

	public override void show(string filepath){
		// Texture2D tex = CLASS.DownloadedContents(filepath)
		// if(f){それくれ}
		// else {
		Action<Texture2D> success_func = (Texture2D tex)=>{
			setTexture(tex);
			//CLASS.addDownloadedContents(tex)
		};
		Action failure_func = () => {   };
		WebManager.instance.getResources(success_func,failure_func,filepath);
		//}

		StartCoroutine (getImage (filepath));
	}

	public override void show (int id)
	{
		
	}

//	public override void show(Item item){
//		//Debug.Log(Item);
//		StartCoroutine (getImage (item.getFilepath()));
//	}

	//imageのオブジェクトに貼り付ける
	public override void setTexture(Texture2D tex){
		Sprite s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		Image image = this.gameObject.GetComponent<Image> ();
		image.sprite = s;
		fullImage.GetComponent<Image> ().sprite = s;

		showCompleted = true;
	}

	public void clear(){
		GetComponent<Image> ().sprite = null;
	}
}

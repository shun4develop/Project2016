using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class ContentsOfImage : ContentsViewerBase {

	private Image image;
	private ImageFullPanel ifp;
	void Start(){
		image = GetComponent<Image> ();
		ifp = GetComponent<ImageFullPanel> ();
	}

	private Coroutine runningCoroutine;

	public override void show(){
		
		Sprite s = ItemData.instance.getContentsSpriteById (Item.getId ());

		if (s != null) {
			setTexture (s);
		} else {
			Action<Texture2D> success_func = (Texture2D tex) => {
				//読み込み開始時と完了時にItemのIDが同じ場合のみ画像を表示する。

				setTexture (tex);
				
				ItemData.instance.addContents(Item.getId(),tex);

			};
			Action failure_func = () => {
				Debug.Log ("ContentsOfImage : failure_func");
			};
			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Album") {
				// Scene が Album の場合は BagItem を参照する
				runningCoroutine = WebManager.instance.getResources (success_func, failure_func, ItemData.instance.getBagItemById (Item.getId ()).getFilepath ());
			} else {
				// それ以外の Scene（Camera）では LocationItem をもらう
				runningCoroutine = WebManager.instance.getResources (success_func, failure_func, ItemData.instance.getLocationItemById (Item.getId ()).getFilepath ());
			}
		}
	}

	//imageのオブジェクトに貼り付ける
	public override void setTexture(Texture2D tex){
		Sprite s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		setTexture (s);
		ItemData.instance.addSprite (Item.getId(), s);
	}
	public  void setTexture(Sprite s){
		image.sprite = s;
		ifp.setSprite (s);
		showCompleted = true;
	}
	public void clearImage(){
		try{
			StopCoroutine (runningCoroutine);
		}catch{
			Debug.Log ("すでにCoroutineが終了しています。");
		}finally{
			image.sprite = null;
			ifp.clearImage ();
			showCompleted = false;
		}

	}
}

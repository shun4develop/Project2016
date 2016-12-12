using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ContentsOfObject : ContentsViewerBase  {

	CanvasCreatorBase cc;
	GameObject contents;

	void Start(){
		//cdc = GetComponent<DetailInfoCanvasCreator> ();
		cc = GetComponent<CanvasCreatorBase>();
		contents = GameObject.Find ("Contents");
	}
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

	//オブジェクトに貼り付ける
	public override void setTexture(Texture2D tex){

		this.gameObject.GetComponent<MeshRenderer> ().material.mainTexture = tex;
	}

	public void OnMouseDown() {
		cc.create (Item);
		contents.SetActive (false);
		Debug.Log ("OnMouseDown" + Item);
	}

}


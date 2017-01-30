using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ContentsOfObject : ContentsViewerBase  {

	CanvasCreatorBase cc;
	CameraViewerController cvc;

	public bool touchFlag = true;

	private Coroutine runningCoroutine;

	void Start(){
		cc = GetComponent<CanvasCreatorBase>();
		cvc = GameObject.Find ("System").GetComponent<CameraViewerController>();
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
			runningCoroutine = WebManager.instance.getResources (success_func, failure_func, ItemData.instance.getLocationItemById (Item.getId()).getThumbnail ());
		}

	}

	//オブジェクトのテクスチャに貼り付ける
	public override void setTexture(Texture2D tex){
		try{
			StopCoroutine (runningCoroutine);
		}catch{
			Debug.Log ("すでにCoroutineが終了しています。");
		}finally{
			this.gameObject.GetComponent<MeshRenderer> ().material.mainTexture = tex;
		}
	}

	public void OnMouseDown() {
		if(touchFlag){
			cc.create (Item);
			cvc.touchFalseFlag ();
		}
	}

}


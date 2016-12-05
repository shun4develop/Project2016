using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContentsOfObject : ContentsViewerBase  {

//	protected GameObject canvas;
	//DetailInfoCanvasCreator cdc;

	CanvasCreatorBase cc;
//	bool touch = false;

	void Start(){
		//cdc = GetComponent<DetailInfoCanvasCreator> ();
		cc = GetComponent<CanvasCreatorBase>();

	}
	public override void show ()
	{
		Debug.Log ("実装してください");
	}
	public override void show(string filepath){
		StartCoroutine (getImage (filepath));
	}

	public override void show(int id){
		StartCoroutine (getImage (ItemData.instance.getItemById(id).getFilepath()));
	}

//	public override void show(Item item){
//		StartCoroutine (getImage (item.getThumbnail()));
//	}


	//オブジェクトに貼り付ける
	public override void setTexture(Texture2D tex){

		this.gameObject.GetComponent<MeshRenderer> ().material.mainTexture = tex;
	}

	public void OnMouseDown() {
		cc.create (Item);
		Debug.Log ("OnMouseDown" + Item);
	}




}


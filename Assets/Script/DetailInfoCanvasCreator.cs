using UnityEngine;
using System.Collections;
using MyClass;
//詳細画面を作るクラス

public class DetailInfoCanvasCreator : CanvasCreatorBase {
	public override void create (Item item){
		canvas = GameObject.Find("DetailPanel");
		canvas.GetComponent<DetailInfoCanvas> ().init (item);
		canvas.GetComponent<AnimationUI> ().slideIn ("RIGHT");
	}
	public override void create (int id){
		//GameObject tmp =  Instantiate (canvas);
		//tmp.GetComponent<DetailInfoCanvas> ().init (id);
	}
}

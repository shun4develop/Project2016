using UnityEngine;
using System.Collections;
using MyClass;
using System.Threading;
//詳細画面を作るクラス

public class DetailInfoCanvasCreator : CanvasCreatorBase {
	public override void create (Item item){
		canvas = GameObject.Find("DetailPanel");
		canvas.GetComponent<DetailInfoCanvas> ().init (item);
		canvas.GetComponent<AnimationUI> ().slideIn ("TOP");
	}
}

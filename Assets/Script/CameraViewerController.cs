using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

using MyClass;
using MyLibrary;


public class CameraViewerController: MonoBehaviour {
	
	public GameObject pre;
	public GameObject canvas;
	public GameObject contents;

	void Start () {
		foreach (Transform child in canvas.transform)
		{
			if (child.GetComponent<AnimationUI> ()) {
				child.GetComponent<AnimationUI> ().slideOut ("TOP");
			}
		}

//		WebManager wm = GetComponent<WebManager>();
		// Action<> 戻り値なし
		// Func<>	戻り値ある

		// Action<T>	 		引数 1
		// Action<T1,T2>		引数 2
		// Func  <T,TResult>	引数 1 戻り値 TResult型
		// Func  <T1,T2,TResult> 同上


		Action<string> positive_func = (string json) => {
			ItemData.instance.SetItems(JsonHelper.ListFromJson<Item> (json));
			contentsInit(ItemData.instance.items);
		};

		Action negative_func = () => {
			//エラー表示
			Debug.Log("CameraViewerController.Start()   エラー");
		};

		WebManager.instance.downloadContents ( positive_func , negative_func, "owner");

	}

	private void contentsInit(List<Item> items){
		//返ってきたデータの分だけItemクラスのリストに入っているので
		//items.Countの数だけ繰り返す

		for (int i=0;i<items.Count;i++){
			Item item = ItemData.instance.items[i];
			//			Debug.Log (item);
			//プレハブを複製
			//GameObject tmp = new GameObject();
			GameObject tmp = Instantiate(pre);
			tmp.transform.SetParent (contents.transform);

			ContentsViewerBase cv = tmp.GetComponent<ContentsViewerBase> ();

			cv.init (item);

			cv.show ();

		}
	}

}


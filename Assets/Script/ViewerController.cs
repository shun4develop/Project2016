using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using MyClass;
using MyLibrary;

public class ViewerController : MonoBehaviour {
	public GameObject pre;

	public GameObject contents;
	public GameObject tx;

	void Start () {

		//WebManager wm = GetComponent<WebManager>();
		// Action<> 戻り値なし
		// Func<>	戻り値ある

		// Action<T>	 		引数 1
		// Action<T1,T2>		引数 2
		// Func  <T,TResult>	引数 1 戻り値 TResult型
		// Func  <T1,T2,TResult> 同上

		downloadContents ();			
	}

	public void downloadContents(){
		Action<string> positive_func = (string text) => {
			//ItemData.instance.SetBagItems(JsonHelper.ListFromJson<Item> (text));
			contentsInit(ItemData.instance.bagItems);
		};

		Action negative_func = () => {
			//エラー表示
			Debug.Log("ViewerController.Start()   エラー");
			tx.SetActive(true);
		};

		WebManager.instance.downloadContents ( positive_func , negative_func, "bag");
	}

	private void contentsInit(List<Item> items){
		//返ってきたデータの分だけItemクラスのリストに入っているので
		//items.Countの数だけ繰り返す

		for (int i=0;i<items.Count;i++){
			Item item = ItemData.instance.bagItems[i];

			GameObject tmp = Instantiate(pre);
			tmp.transform.SetParent (contents.transform);

			ContentsViewerBase cv = tmp.GetComponent<ContentsViewerBase> ();
			cv.init (item);
			
			cv.show ();

		}
		Debug.Log ("bagItems / " + ItemData.instance.bagItems.Count);
		Debug.Log ("locationItems / " + ItemData.instance.locationItems.Count);
	}

	public void contentsUpdate(){
		foreach (Transform child in contents.transform)
		{
			Destroy (child.gameObject);
		}
		downloadContents ();
	}
}

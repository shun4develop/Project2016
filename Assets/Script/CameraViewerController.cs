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

	private string lat;
	private string lon;

	void Start () {
		LocationInfo locationInfo = LocationManager.location;
		lat = locationInfo.latitude.ToString("F6");
		lon = locationInfo.latitude.ToString("F6");

//		foreach (Transform child in canvas.transform)
//		{
//			if (child.GetComponent<AnimationUI> ()) {
//				child.GetComponent<AnimationUI> ().slideOut ("TOP");
//			}
//		}
		// Action<> 戻り値なし
		// Func<>	戻り値ある

		// Action<T>	 		引数 1
		// Action<T1,T2>		引数 2
		// Func  <T,TResult>	引数 1 戻り値 TResult型
		// Func  <T1,T2,TResult> 同上
		Action<string> positive_func2 = (string text) => {
			ItemData.instance.SetBagItems(JsonHelper.ListFromJson<Item> (text));

			Action<string> positive_func = (string json) => {
				ItemData.instance.SetItems(JsonHelper.ListFromJson<Item> (json));
				contentsInit(ItemData.instance.locationItems);
			};

			Action negative_func = () => {
				//エラー表示
				Debug.Log("CameraViewerController.Start()   エラー");
			};

			WebManager.instance.downloadContents ( positive_func , negative_func, lat, lon);
//			WebManager.instance.downloadContents ( positive_func , negative_func, "owner");
		};

		Action negative_func2 = () => {
			//エラー表示
			Debug.Log("ViewerController.Start()   エラー");
		};

		WebManager.instance.downloadContents ( positive_func2 , negative_func2, "bag");

	}
		
	private void contentsInit(List<Item> items){

		Debug.Log (lat + " : " + lon);

		//返ってきたデータの分だけItemクラスのリストに入っているので
		//items.Countの数だけ繰り返す
		for (int i=0;i<items.Count;i++){
			Item item = ItemData.instance.locationItems[i];
			GameObject tmp = Instantiate(pre);
			tmp.transform.SetParent (contents.transform);
			tmp.name = item.getId ().ToString();
			ContentsViewerBase cv = tmp.GetComponent<ContentsViewerBase> ();

			cv.init (item);

			cv.show ();
		}
		Debug.Log ("bagItems / " + ItemData.instance.bagItems.Count);
		Debug.Log ("locationItems / " + ItemData.instance.locationItems.Count);
	}

	public void touchFalseFlag(){
		foreach (Transform child in contents.transform)
		{
			child.GetComponent<ContentsOfObject> ().touchFlag = false;
		}
	}

	public void touchTrueFlag(){
		foreach (Transform child in contents.transform)
		{
			child.GetComponent<ContentsOfObject> ().touchFlag = true;
		}
	}

}


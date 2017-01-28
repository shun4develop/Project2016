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


	private Dictionary<int,GameObject> instanceObjectList = new Dictionary<int,GameObject> ();
	//Start with this method
	IEnumerator Start(){
		//位置情報の更新がスタートするまで待つ
		while (LocationManager.location.latitude == 0 
			&& LocationManager.location.longitude == 0) {
			yield return new WaitForSeconds (1);
		}
		//15秒ごとにシーンないのコンテンツを更新する
		while (true) {
			updateContents ();
			yield return new WaitForSeconds (15);
		}
	}
	//更新処理
	public void updateContents(){
		lat = LocationManager.location.latitude.ToString ();
		lon = LocationManager.location.longitude.ToString ();

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
		Action<string> positive_func = (string text) => {
			ItemData.instance.SetBagItems (JsonHelper.ListFromJson<Item> (text));

			Action<string> positive_func2 = (string json) => {
				ItemData.instance.SetLocationItems (JsonHelper.ListFromJson<Item> (json));
				contentsInit ();
			};

			Action negative_func2 = () => {
				//エラー表示
				Debug.Log("CameraViewerController.Start()   エラー");
			};

			WebManager.instance.downloadContents ( positive_func2 , negative_func2, "bag");

			//yield return new WaitForSeconds (5);
		};
		Action negative_func = () => {
				
		};
		WebManager.instance.downloadContents (positive_func,negative_func,lat,lon);
	}

	//現在のコンテンツ情報をもとにオブジェクトを削除、生成する
	private void contentsInit(){
		
		//返ってきたデータの分だけItemクラスのリストに入っているので
		//items.Countの数だけ繰り返す

		foreach(Item item in ItemData.instance.locationItems){
			if (instanceObjectList.ContainsKey (item.getId ()))
				continue;
			
			GameObject tmp = Instantiate (pre);
			tmp.transform.SetParent (contents.transform);
			tmp.name = item.getId ().ToString ();

			ContentsViewerBase cv = tmp.GetComponent<ContentsViewerBase> ();

			cv.init (item);

			cv.show ();

			instanceObjectList.Add (item.getId (), tmp);

		}
		foreach(int key in instanceObjectList.Keys){
			bool find = false;
			foreach(Item item in ItemData.instance.locationItems){
				if (item.getId () == key) {
					find = true;
				}
			}
			if (!find) {
				GameObject obj;
				instanceObjectList.TryGetValue (key, out obj);
				if (obj != null) {
					Debug.Log (obj.name + " Destroy");
					Destroy (obj);
				}
			}
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


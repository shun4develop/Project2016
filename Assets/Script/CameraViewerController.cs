﻿using UnityEngine;
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
	public CanvasGroup contentsFailure;

	private string lat;
	private string lon;

	private Coroutine runningCoroutine;

	private Dictionary<int,GameObject> instanceObjectList = new Dictionary<int,GameObject> ();
	void Start(){
		runningCoroutine = StartCoroutine(start());
	}
	private void updateScene(){
		Debug.Log ("update");
		if(runningCoroutine != null){
			StopCoroutine (runningCoroutine);
		}
		runningCoroutine = StartCoroutine (start());
	}
	//Start with this method
	IEnumerator start(){
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

			WebManager.instance.downloadContents ( positive_func2 , negative_func2, lat,lon);

			//yield return new WaitForSeconds (5);
		};
		Action negative_func = () => {
				
		};
		WebManager.instance.downloadContents (positive_func,negative_func,"bag");
	}

	//現在のコンテンツ情報をもとにオブジェクトを削除、生成する
	private void contentsInit(){
		//返ってきたデータの分だけItemクラスのリストに入っているので
		//items.Countの数だけ繰り返す
		foreach(Item item in ItemData.instance.locationItems){
			if(string.IsNullOrEmpty (item.getFilepath())){
				destroyObject (item.getId());
				continue;
			}
			//すでに作成済み
			if (instanceObjectList.ContainsKey (item.getId ())) {
				continue;
			}

			
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
				destroyObject (key);
			}
		}

		if (instanceObjectList.Count == 0) {
			contentsFailure.alpha = 1f;
			contentsFailure.interactable = true;
			contentsFailure.blocksRaycasts = true;
		} else {
			contentsFailure.alpha = 0f;
			contentsFailure.interactable = false;
			contentsFailure.blocksRaycasts = false;
		}

		Debug.Log ("bagItems / " + ItemData.instance.bagItems.Count);
		Debug.Log ("locationItems / " + ItemData.instance.locationItems.Count);
	}
	private void destroyObject(int key){
		GameObject obj;
		instanceObjectList.TryGetValue (key, out obj);
		instanceObjectList.Remove (key);
		if (obj != null) {
			Destroy (obj);
		}
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


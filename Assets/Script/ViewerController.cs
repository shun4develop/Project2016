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

	public ScrollRect scrollView;
	public Text message;
	public Button goToMap;

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
			ItemData.instance.SetBagItems(JsonHelper.ListFromJson<Item> (text));
			if(ItemData.instance.bagItems.Count == 0){
				message.gameObject.SetActive(true);
				message.text = "コンテンツがありません。";
				goToMap.gameObject.SetActive(true);
				goToMap.enabled = true;
				message.enabled = true;
			}
			contentsInit();

		};

		Action negative_func = () => {
			//エラー表示
			Debug.Log("ViewerController.Start()   エラー");
			message.gameObject.SetActive(true);
			message.text = "コンテンツの取得に失敗しました。";
		};

		WebManager.instance.downloadContents ( positive_func , negative_func, "bag");
	}

	private void contentsInit(){
		//返ってきたデータの分だけItemクラスのリストに入っているので
		//items.Countの数だけ繰り返す
		for (int i=ItemData.instance.bagItems.Count-1;i>=0;i--){
			Item item = ItemData.instance.bagItems[i];

			GameObject tmp = Instantiate(pre);

//			if (tmp == null)
//				return;

			tmp.transform.SetParent (scrollView.content.transform);

			ContentsViewerBase cv = tmp.GetComponent<ContentsViewerBase> ();
			cv.init (item);
			
			cv.show ();
		}
		Debug.Log ("bagItems / " + ItemData.instance.bagItems.Count);
		Debug.Log ("locationItems / " + ItemData.instance.locationItems.Count);
	}

	public void contentsUpdate(){
		foreach (Transform child in scrollView.content.transform)
		{
			Destroy (child.gameObject);
		}
		downloadContents ();
	}
}

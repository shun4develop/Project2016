using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using MyClass;
using MyLibrary;

public class ConfirmationButton : MonoBehaviour {

	public GameObject detail;

	public void contentDelete(){

		ViewerController vc = GameObject.Find ("System").GetComponent<ViewerController>();
		Item item = detail.GetComponent<DetailInfoCanvas>().item;

		Action<string> positive_func = (string text) => {
			ItemData.instance.deleteContentById(item.getId());
//			item.setPermitSave(true);
			vc.contentsUpdate ();
			Debug.Log("削除完了");
		};

		Action negative_func = () => {
			//エラー表示
			Debug.Log("エラー" + item.getId() + item.getTitle());
		};
		WebManager.instance.contentsDump (positive_func, negative_func, item.getId(), item.getTitle());
	}

	public void contentSave(){
		Item item = detail.GetComponent<DetailInfoCanvas>().item;
		Action<string> positive_func = (string text) => {
//			item.setPermitSave(false);
			ItemData.instance.saveContent(item);
			Debug.Log("保存完了");
		};

		Action negative_func = () => {
			//エラー表示
			Debug.Log("エラー / " + item);
		};
		WebManager.instance.contentsTaken (positive_func, negative_func, item.getId(), item.getTitle());
	}




}
